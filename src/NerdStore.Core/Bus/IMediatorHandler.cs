using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
    }
}
