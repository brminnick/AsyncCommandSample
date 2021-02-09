using Xamarin.CommunityToolkit.Markup;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AsyncCommandSample
{
    class XamarinCommunityToolkitInfoPage : ContentPage
    {
        const string _xamarinCommunityToolkitUrl = "https://github.com/xamarin/XamarinCommunityToolkit/blob/1.0.2/assets/XamarinCommunityToolkit.png?raw=true";

        readonly IMainThread _mainThread;
        readonly XamarinCommunityToolkitInfoViewModel _viewModel;

        public XamarinCommunityToolkitInfoPage(IMainThread mainThread)
        {
            _mainThread = mainThread;

            BindingContext = _viewModel = new XamarinCommunityToolkitInfoViewModel(new PreferencesImplementation());
            _viewModel.GetLatestReleaseFailed += HandleGetLatestReleaseFailed;

            Content = new StackLayout
            {
                Children =
                {
                    new Image { Source = _xamarinCommunityToolkitUrl, HeightRequest = 250 }.Center(),

                    new Label().Center().TextCenter()
                        .Bind<Label, string? , string>(Label.TextProperty, nameof(XamarinCommunityToolkitInfoViewModel.LatestRelease), convert: latestRelease => $"Latest Release: {latestRelease ?? "Unknown"}"),

                    new Button { Text = "Get Latest Version" }.Center()
                        .Bind(Button.CommandProperty, nameof(XamarinCommunityToolkitInfoViewModel.GetLatestRelease))
                }
            }.Center();
        }

        void HandleGetLatestReleaseFailed(object sender, string message) =>
            _mainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Failed to Retrieve Latest Version", message, "OK"));
    }
}
