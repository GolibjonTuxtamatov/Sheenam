using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class GuestDependencyValidationException : Xeption
    {
        public GuestDependencyValidationException(Xeption innerException)
            :base(
                 message:"Guest dependency validation error occured, fix the errors and try again",
                 innerException)
        {}
    }
}
