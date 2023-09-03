//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foudations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependecyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            SqlException sqlException = GetSqlError();
            var failedStorageGuestException = new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedStorageGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestAsync(someGuest))
                .ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestServic.AddGuestAsync(someGuest);

            //then
            await Assert.ThrowsAsync<GuestDependencyException>(() =>
                addGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(someGuest),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDuplicateKeyDependencyExceptionOnAddIfGuestIsExistAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            string randomString = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomString);

            var alreadyExistGuestException =
                new AlreadyExistGuestException(duplicateKeyException);

            var expectedGuestDependencyValidationException =
                new GuestDependencyValidationException(alreadyExistGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestAsync(someGuest))
                .ThrowsAsync(duplicateKeyException);

            //when
            ValueTask<Guest> addGuest =
                this.guestServic.AddGuestAsync(someGuest);

            //then
            await Assert.ThrowsAsync<GuestDependencyValidationException>(() =>
                addGuest.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(someGuest),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyValidationException))),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
