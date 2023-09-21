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
        public void ShouldGuestsRetrieveAllAsync()
        {
            //given

            IQueryable<Guest> storageGuests =
                CreateRandomGuests();

            var expectedGuests = storageGuests.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                .Returns(storageGuests);

            //when
            IQueryable<Guest> actualGuests =
                this.guestServic.RetrieveAllGuests();

            //then
            actualGuests.Should().BeEquivalentTo(expectedGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
