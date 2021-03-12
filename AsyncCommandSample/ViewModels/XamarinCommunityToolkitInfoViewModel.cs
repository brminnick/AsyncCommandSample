using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials.Interfaces;

namespace AsyncCommandSample
{
    public class XamarinCommunityToolkitInfoViewModel : BaseViewModel
    {
        readonly static HttpClient _gitHubClient = CreateHttpClient();

        readonly WeakEventManager<string> _getLatestReleaseFailedEventManager = new();
        readonly IPreferences _preferences;

        public XamarinCommunityToolkitInfoViewModel(IPreferences preferences)
        {
            _preferences = preferences;
            GetLatestRelease = new AsyncCommand(ExecuteGetLatestRelease, allowsMultipleExecutions: false);
        }

        public event EventHandler<string> GetLatestReleaseFailed
        {
            add => _getLatestReleaseFailedEventManager.AddEventHandler(value);
            remove => _getLatestReleaseFailedEventManager.RemoveEventHandler(value);
        }

        public IAsyncCommand GetLatestRelease { get; }

        public string? LatestRelease
        {
            get => _preferences.Get(nameof(LatestRelease), null);
            set
            {
                if (value is not null)
                {
                    _preferences.Set(nameof(LatestRelease), value);
                    OnPropertyChanged();
                }
            }
        }

        static HttpClient CreateHttpClient()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.github.com")
            };
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(nameof(AsyncCommandSample))));

            return client;
        }

        async Task ExecuteGetLatestRelease()
        {
            try
            {
                var releasesJson = await _gitHubClient.GetStringAsync("/repos/xamarin/XamarinCommunityToolkit/releases").ConfigureAwait(false);
                var releases = JsonConvert.DeserializeObject<IReadOnlyList<GitHubReleasesModel>>(releasesJson);

                LatestRelease = releases.First().Tag_Name;
            }
            catch (Exception e)
            {
                OnGetLatestReleaseFailed(e.Message);
            }
        }

        void OnGetLatestReleaseFailed(string message) => _getLatestReleaseFailedEventManager.RaiseEvent(this, message, nameof(GetLatestReleaseFailed));
    }
}
