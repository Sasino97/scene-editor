/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    public class DffParsingException : Exception
    {
        private const string ERROR_MESSAGE = "An error has occurred during the parsing of a DFF file.";

        public DffParsingException()
            : base(ERROR_MESSAGE) { }

        public DffParsingException(string errorMessage)
            : base(errorMessage) { }
    }
}
