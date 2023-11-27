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
            Assert.Multiple(() => 
            {
                Assert.That(latestRelease_Initial, Is.Not.Null);
                Assert.That(latestRelease_Final, Is.Not.Null);
            });

            void HandleGetLatestReleaseFailed(object? sender, string e)
            {
                getLatestReleaseFailedTCS.SetResult(e);
            }
        }

    }
}