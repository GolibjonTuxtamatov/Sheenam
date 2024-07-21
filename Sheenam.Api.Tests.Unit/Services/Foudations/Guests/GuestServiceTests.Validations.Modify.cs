using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foudations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsNullAndLogItAsync()
        {
            //given
            Guest nullGuest = null;
            var nullGuestException = new NullGuestException();

            var expectedGuestValidationException =
                new GuestValidationException(nullGuestException);

            //given
            ValueTask<Guest> modifyGuestTask = this.guestServic.ModifyGuestAsync(nullGuest);

            await Assert.ThrowsAsync<GuestValidationException>(modifyGuestTask.AsTask);

            //then

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            Guest guest = new()
            {
                FirstName = invalidText
            };

            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            invalidGuestException.AddData(
                key: nameof(Guest.FirstName),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.LastName),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Email),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Address),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.DateOfBirth),
                values: "Date is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServic.ModifyGuestAsync(guest);

            await Assert.ThrowsAsync<GuestValidationException>(modifyGuestTask.AsTask);

            //then

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGenderIsInvalidAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest invalidGuest = randomGuest;
            invalidGuest.Gender = GetInvalidEnum<GenderType>();
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Gender),
                values: "Value is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServic.ModifyGuestAsync(invalidGuest);

            //then
            await Assert.ThrowsAsync<GuestValidationException>(
                modifyGuestTask.AsTask);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExcetionOnModifyIfGuestDoesntExistAndLogItAsync()
        {
            //given
            Guest notExistGuest = CreateRandomGuest();
            Guest nullGuest = null;

            var notFoundGuetException = new NotFoundGuestException(notExistGuest.Id);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(notExistGuest.Id))
                    .ReturnsAsync(nullGuest);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuetException);

            //when
            ValueTask<Guest> retrieveGuestyIdTask =
                this.guestServic.ModifyGuestAsync(notExistGuest);

            await Assert.ThrowsAsync<GuestValidationException>(retrieveGuestyIdTask.AsTask);

            //then
            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(notExistGuest.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
