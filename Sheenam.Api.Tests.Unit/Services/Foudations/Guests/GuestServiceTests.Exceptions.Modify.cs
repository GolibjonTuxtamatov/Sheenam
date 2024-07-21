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
        public async Task ShouldThrowCriticalDependecyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            SqlException sqlException = GetSqlError();
            var failedStorageGuestException = new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedStorageGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someGuest.Id))
                    .ReturnsAsync(someGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(someGuest))
                .ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestServic.ModifyGuestAsync(someGuest);

            //then
            await Assert.ThrowsAsync<GuestDependencyException>(
                addGuestTask.AsTask);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someGuest.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(someGuest),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            string randomString = GetRandomString();

            var exception = new Exception(randomString);

            var failedServiceException = new FailedServiceException(exception);

            var expectedGuestServiceException = new GuestServiceException(failedServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someGuest.Id))
                    .ReturnsAsync(someGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(someGuest))
                .ThrowsAsync(exception);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestServic.ModifyGuestAsync(someGuest);

            //then
            await Assert.ThrowsAsync<GuestServiceException>(
                addGuestTask.AsTask);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someGuest.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(someGuest),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestServiceException))),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
