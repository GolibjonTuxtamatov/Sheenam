﻿//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuestService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
            TryCatch(async () =>
            {
                ValidateGuestOnAdd(guest);

                return await this.storageBroker.InsertGuestAsync(guest);
            });

        public IQueryable<Guest> RetrieveAllGuests() =>
            TryCatch(() => this.storageBroker.SelectAllGuests());

        public ValueTask<Guest> RetrieveGuestByIdAsync(Guid id) =>
            TryCatch(async () =>
            {
                ValidateGuestId(id);

                Guest maybeGuest = await this.storageBroker.SelectGuestByIdAsync(id);

                ValidateStorageGuestToExists(maybeGuest, id);

                return maybeGuest;
            });


        public ValueTask<Guest> ModifyGuestAsync(Guest guest) =>
            TryCatch(async () =>
            {
                ValidateGuestOnModify(guest);

                Guest maybeGuest =
                    await this.storageBroker.SelectGuestByIdAsync(guest.Id);

                ValidateStorageGuestToExists(maybeGuest, guest.Id);

                return await this.storageBroker.UpdateGuestAsync(guest);
            });

        public async ValueTask<Guest> DeleteGuestAsync(Guid id)
        {
            Guest maybeGuest = await this.storageBroker.SelectGuestByIdAsync(id);

            return await this.storageBroker.DeleteGuestAsync(maybeGuest);
        }
    }
}
