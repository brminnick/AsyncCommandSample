using Xamarin.Essentials.Implementation;
using Xamarin.Forms;

namespace AsyncCommandSample
{
    public class App : Application
    {
        public App() => MainPage = new XamarinCommunityToolkitInfoPage(new MainThreadImplementation());
    }
}
