/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

using Sasinosoft.SampMapEditor.Vehicles;

namespace Sasinosoft.SampMapEditor.IDE
{
    public class VehicleDefinition
    {
        public string ModelName { get; set; }
        public string TextureDictionaryName { get; set; }
        public VehicleType Type { get; set; }
        public float WheelScaleFront { get; set; }
        public float WheelScaleRear { get; set; }
    }
}
