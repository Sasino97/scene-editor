using System;

namespace Sasinosoft.SampMapEditor.IMG
{
    public class UnknownIMGVersionException : Exception
    {
        private const string ERROR_MESSAGE = "Unknown IMG Archive version.";

        public UnknownIMGVersionException()
            : base(ERROR_MESSAGE) { }
    }
}
