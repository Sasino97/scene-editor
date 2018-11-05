/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public abstract class Section
    {
        public SectionType Type = SectionType.None;
        public UInt32 Size;
        public UInt32 Version;
        public ExtendedSection Parent;

        public override string ToString() // helpful for debugger
        {
            return Type.ToString();
        }
    }
}
