/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.IDE
{
    /// <summary>
    /// Represent an "objs" IDE section.
    /// </summary>
    /// <see cref="https://gtamods.com/wiki/OBJS"/>
    public class ObjectsIDESection : IDESection
    {
        public UInt32 Id;
        public string ModelName;
        public string TextureDictionaryName;
        public UInt32 MeshCount;
        public float DrawDistance1;
        public float DrawDistance2; // ignored 
        public float DrawDistance3; // ignored 
        public IDEFlags Flags;
    }
}
