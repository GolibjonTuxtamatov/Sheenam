//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntityEntr =
                await broker.Guests.AddAsync(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntr.Entity;
        }

        public IQueryable<Guest> SelectAllGuests()
        {
            using var broker = new StorageBroker(this.configuration);

            return broker.Guests;
        }

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid id)
        {
            using var broker = new StorageBroker(this.configuration);

            Guest guest =
                await broker.Guests.FirstOrDefaultAsync(p => p.Id == id);

            return guest;
        }

        public async ValueTask<Guest> UpdateGuestAsync(Guid id)
        {
            using var broker = new StorageBroker(this.configuration);

            Guest guest = await broker.Guests.FirstOrDefaultAsync(p => p.Id == id);

            guest.Status = ItemState.Update;
            await broker.SaveChangesAsync();

            return guest;
        }
    }
}
