/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.RenderWare.Txd
{
    public enum TextureFormats : UInt32
    {
        Unknown = 0x00000000,
        DXT1    = 0x31545844,
        DXT3    = 0x33545844
    }
}
