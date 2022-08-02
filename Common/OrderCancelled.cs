using System;

namespace Hydrogen.Common
{
    public class OrderCancelled
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
    }
}