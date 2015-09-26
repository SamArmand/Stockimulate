using System;
using System.Runtime.Serialization;

namespace Stockimulate
{
    [Serializable]
    internal class TradeCreationException : Exception
    {
        public TradeCreationException()
        {
        }

        public TradeCreationException(string message) : base(message)
        {
        }

        public TradeCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TradeCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}