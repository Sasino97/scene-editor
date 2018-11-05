/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.IMG
{
    public struct DirEntry
    {
        public UInt32 Offset;
        public UInt16 Size;
        public UInt16 Size2;
        public string FileName; // 24 bytes
    }
}
