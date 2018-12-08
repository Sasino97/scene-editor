/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
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
