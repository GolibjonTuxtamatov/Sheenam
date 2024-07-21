//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Host> Hosts { get; set; }

        public async ValueTask<Host> InsertHostAsync(Host host) =>
            await InsertAsync(host);

    }
}
