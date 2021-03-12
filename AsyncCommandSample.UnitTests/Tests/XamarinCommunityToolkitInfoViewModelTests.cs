using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncCommandSample.UnitTests
{
    class XamarinCommunityToolkitInfoViewModelTests
    {
        [Test]
        public async Task GetLatestReleaseTest()
        {
            // Arrange
            string? latestRelease_Initial, latestRelease_Final;

            var xamarinCommunityToolkitInfoViewModel = new XamarinCommunityToolkitInfoViewModel(new MockPreferences());

            // Act
            latestRelease_Initial = xamarinCommunityToolkitInfoViewModel.LatestRelease;

            await xamarinCommunityToolkitInfoViewModel.GetLatestRelease.ExecuteAsync().ConfigureAwait(false);

            latestRelease_Final = xamarinCommunityToolkitInfoViewModel.LatestRelease;

            // Assert
            Assert.IsNull(latestRelease_Initial);
            Assert.IsNotNull(latestRelease_Final);
        }
    }
}