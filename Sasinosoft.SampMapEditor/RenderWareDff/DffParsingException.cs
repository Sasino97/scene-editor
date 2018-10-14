using System;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class DffParsingException : Exception
    {
        private const string ERROR_MESSAGE = "An error has occurred during the parsing of a Dff file.";

        public DffParsingException()
            : base(ERROR_MESSAGE) { }

        public DffParsingException(string errorMessage)
            : base(errorMessage) { }
    }
}
