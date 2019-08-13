using MediatR;
using System;

namespace NerdStore.Core.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; protected set; }
        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}