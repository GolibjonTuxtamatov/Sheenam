using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public  async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someGuid = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedStorgeGuestEception =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedStorgeGuestEception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someGuid))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> retrieveByIdGuestTask =
                this.guestServic.RetrieveGuestByIdAsync(someGuid);

            //then
            await Assert.ThrowsAsync<GuestDependencyException>(retrieveByIdGuestTask.AsTask);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someGuid), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
