/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

using Sasinosoft.SampMapEditor.Pedestrians;

namespace Sasinosoft.SampMapEditor.Data
{
    public class SkinDefinition
    {
        public string ModelName { get; set; }
        public string TextureDictionaryName { get; set; }
        public string AnimationGroupName { get; set; }
        public PedestrianType Type { get; set; }
    }
}
