using System;
using System.Runtime.Serialization;

namespace FornecedoresEmpresa.Utils
{
    [Serializable]
    public class UsuarioException : Exception, ISerializable
    {
        public UsuarioException() : base() {}

        public UsuarioException(string message) : base(message) { }

        protected UsuarioException(SerializationInfo info, StreamingContext context) { }
    }
}
