//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using System;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class FailedGuestStorageException : Xeption
    {
        public FailedGuestStorageException(Exception innerException)
            : base(
                 message: "Failed guest storage error occured, contact support",
                 innerException)
        { }
    }
}
