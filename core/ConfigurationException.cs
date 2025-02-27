using System;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.core
{
    [Serializable]
    internal class ConfigurationException : Exception
    {
        public ConfigurationException()
            : base()
        { /*** empty ***/ }

        public ConfigurationException( string message )
            : base( message )
        { /*** empty ***/ }

        public ConfigurationException( string message, Exception innerException )
            : base( message, innerException )
        { /*** empty ***/ }

        protected ConfigurationException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        { /*** empty ***/ }
    }
}