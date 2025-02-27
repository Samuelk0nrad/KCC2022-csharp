using com.knapp.CodingContest.data;
using System;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class OrderIncompleteException : WarehouseException
    {
        public OrderIncompleteException( Order order)
            : base($"not yet completed: {order}")
        {
        }
        public OrderIncompleteException(string message) : base(message)
        {
        }

        public OrderIncompleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderIncompleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
