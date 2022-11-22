using System;
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

            var getLatestReleaseFailedTCS = new TaskCompletionSource<string>();

            var xamarinCommunityToolkitInfoViewModel = new XamarinCommunityToolkitInfoViewModel(new MockPreferences());
            xamarinCommunityToolkitInfoViewModel.GetLatestReleaseFailed += HandleGetLatestReleaseFailed;

            // Act
            latestRelease_Initial = xamarinCommunityToolkitInfoViewModel.LatestRelease;

            await xamarinCommunityToolkitInfoViewModel.GetLatestRelease.ExecuteAsync().ConfigureAwait(false);

            latestRelease_Final = xamarinCommunityToolkitInfoViewModel.LatestRelease ?? await getLatestReleaseFailedTCS.Task.ConfigureAwait(false);

            // Assert
            Assert.IsNull(latestRelease_Initial);
            Assert.IsNotNull(latestRelease_Final, latestRelease_Final );

            void HandleGetLatestReleaseFailed(object? sender, string e)
            {
                getLatestReleaseFailedTCS.SetResult(e);
            }
        }

    }
}