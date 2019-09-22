/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using Sasinosoft.SampMapEditor.Pedestrians;

namespace Sasinosoft.SampMapEditor.IDE
{
    /// <summary>
    /// Represents a "peds" IDE element.
    /// </summary>
    /// <see cref="https://gtamods.com/wiki/PEDS"/>
    public class PedsIDEElement : IDEElement
    {
        public UInt32 Id;
        public string ModelName;
        public string TextureDictionaryName;
        public PedestrianType Type;
        public string Behavior;
        public string AnimationGroup;
        public string CarsCanDrive;
        public UInt32 Flags;
        public string AnimationFile;
        public UInt32 Radio1;
        public UInt32 Radio2;
        public string VoiceArchive;
        public string Voice1;
        public string Voice2;
    }
}
