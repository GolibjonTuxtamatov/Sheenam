//==================================================
// CopyRight (c) Coalition of Good-Hearted Engineers
// Free to Use to Find Comfort and Peace
//==================================================


using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Host> Hosts { get; set; }


    }
}
