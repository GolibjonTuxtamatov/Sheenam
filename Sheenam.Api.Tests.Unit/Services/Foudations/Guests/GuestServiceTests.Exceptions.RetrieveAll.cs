//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foudations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldThrowCrirticalDependencyExceptionOnGetIfSqlErrorOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlError();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                .Throws(sqlException);

            //when
            Action actualGuests = () =>
                this.guestServic.RetrieveAllGuests();

            //then
            Assert.Throws<GuestDependencyException>(actualGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnGetIfServiceErrorOccuresAndLogIt()
        {
            //given
            IQueryable<Guest> someGuests = CreateRandomGuests();
            var serviceException = new Exception();

            var failedServiceException =
                new FailedServiceException(serviceException);

            var expectedGuestServiceException =
                new GuestServiceException(failedServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests()).Throws(expectedGuestServiceException);

            //when
            Action actualGuests = () => this.guestServic.RetrieveAllGuests();

            //then
            Assert.Throws<GuestServiceException>(actualGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestServiceException))),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
