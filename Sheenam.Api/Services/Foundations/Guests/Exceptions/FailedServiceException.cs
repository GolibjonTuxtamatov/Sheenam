//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using System;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class FailedServiceException : Xeption
    {
        public FailedServiceException(Exception innerException)
            : base(message: "Failde guest service error occured, contact support",
                 innerException)
        { }
    }
}
