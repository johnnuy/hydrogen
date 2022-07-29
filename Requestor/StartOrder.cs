using System;

namespace Hydrogen
{
    public interface StartOrder
    {
        Guid Id { get; }
        string OrderNumber { get; }
    }
}