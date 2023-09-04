using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class GuestServiceException :Xeption
    {
        public GuestServiceException(Xeption innerException)
            :base(message:"Guest Service errror occured, contact support",
                 innerException)
        { }
    }
}
