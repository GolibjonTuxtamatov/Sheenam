//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using System;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class AlreadyGuestExistException : Xeption
    {
        public AlreadyGuestExistException(Exception innerException)
            : base(message:"Guest already exist",
                  innerException)
        {}
    }
}
