using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Models.Foundations.Guests;
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
    }
}
