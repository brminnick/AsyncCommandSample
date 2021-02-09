using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace AsyncCommandSample
{
    public class XamarinCommunityToolkitInfoViewModel : BaseViewModel
    {
        readonly static HttpClient _gitHubClient = new()
        {
            BaseAddress = new Uri("https://api.github.com")
        };

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

        async Task ExecuteGetLatestRelease()
        {
            try
            {
                var releasesJson = await _gitHubClient.GetStringAsync("/repos/xamarin/XamarinCommunityToolkit/releases");
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
