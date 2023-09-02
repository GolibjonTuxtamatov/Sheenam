//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foudations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldGuestAddAsync()
        {
            //given
            Guest randomGest = CreateRandomGuest();
            Guest inputGuest = randomGest;
            Guest returningGuest = inputGuest;
            Guest expectedGuest = returningGuest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestAsync(inputGuest))
                .ReturnsAsync(returningGuest);

            //when
            Guest actualGuest =
                await this.guestServic.AddGuestAsync(inputGuest);

            //then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(inputGuest),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

