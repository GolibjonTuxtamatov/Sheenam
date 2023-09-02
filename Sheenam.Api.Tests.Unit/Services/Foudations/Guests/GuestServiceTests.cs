//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using Moq;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using Tynamix.ObjectFiller;

namespace Sheenam.Api.Tests.Unit.Services.Foudations.Guests
{
    public partial class GuestServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IGuestService guestServic;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.guestServic =
                new GuestService(storageBroker: this.storageBrokerMock.Object);
        }

        private static Guest CreateRandomGuest() =>
            CreateGuestFiller(date: GetRandomDateTimeOffcet()).Create();

        private static Filler<Guest> CreateGuestFiller(DateTimeOffset date)
        {
            var filler = new Filler<Guest>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }

        private static DateTimeOffset GetRandomDateTimeOffcet() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
