using System;

namespace Sasinosoft.SampMapEditor.RenderWare.Txd
{
    public class TxdParsingException : Exception
    {
        private const string ERROR_MESSAGE = "An error has occurred during the parsing of a TXD file.";

        public TxdParsingException()
            : base(ERROR_MESSAGE) { }

        public TxdParsingException(string errorMessage)
            : base(errorMessage) { }
    }
}
