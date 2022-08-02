using System;

namespace Hydrogen
{
    public interface CancelOrder
    {
        Guid Id { get; }
        string OrderNumber { get; }
    }
}