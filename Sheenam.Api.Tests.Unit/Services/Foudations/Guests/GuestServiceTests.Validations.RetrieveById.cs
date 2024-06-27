using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foudations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInavalidAndLogItAsync()
        {
            //given
            Guid invalidGuestId = Guid.Empty;

            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> retrieveByIdGuestTask =
                this.guestServic.RetrieveGuestByIdAsync(invalidGuestId);

                await Assert.ThrowsAsync<GuestValidationException>(retrieveByIdGuestTask.AsTask);

            //then

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException)))
                    ,Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExcetionOnRetrieveByIdIfGuestNotFoundAndLogItAsync()
        {
            //given
            Guid someGuid = Guid.NewGuid();
            Guest nullGuest = null;

            var notFoundGuetException = new NotFoundGuestException(someGuid);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuetException);

            //when
            ValueTask<Guest> retrieveGuestyIdTask =
                this.guestServic.RetrieveGuestByIdAsync(someGuid);

                await Assert.ThrowsAsync<GuestValidationException>(retrieveGuestyIdTask.AsTask);

            //then
            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someGuid), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
