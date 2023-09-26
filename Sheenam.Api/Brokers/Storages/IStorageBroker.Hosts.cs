using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Host> InsertHostAsync(Host host);
    }
}
