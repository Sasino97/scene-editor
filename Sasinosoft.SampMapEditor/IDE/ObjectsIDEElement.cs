/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.IDE
{
    /// <summary>
    /// Represents "objs", "tobj", "anim" and "tanm" IDE elements.
    /// </summary>
    /// <see cref="https://gtamods.com/wiki/OBJS"/>
    /// <see cref="https://gtamods.com/wiki/TOBJ"/>
    /// <see cref="https://gtamods.com/wiki/ANIM"/>
    public class ObjectsIDEElement : IDEElement
    {
        public UInt32 Id;
        public string ModelName;
        public string TextureDictionaryName;
        public float DrawDistance;
        public IDEFlags Flags;

        // tobj and tanm
        public UInt32 TimeOn;
        public UInt32 TimeOff;

        // anim and tanm
        public string AnimationName;
    }
}
