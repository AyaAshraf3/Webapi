using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_consumer_.theModel;

namespace WorkerService_consumer_.minimalAPI
{
    public interface IClientConsume
    {
        Task<IEnumerable<submitContent>> GetAllClientConsumesAsync();
    }
}
