/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.Utils;

namespace Sasinosoft.SampMapEditor.Vehicles
{
    /// <summary>
    /// Vehicle types.
    /// </summary>
    public enum VehicleType
    {
        [DataName("car")]
        Car,

        [DataName("bike")]
        Motorbike,

        [DataName("bmx")]
        Bicycle,

        [DataName("boat")]
        Boat,

        [DataName("plane")]
        Plane,

        [DataName("heli")]
        Heli,

        [DataName("quad")]
        Quad,

        [DataName("mtruck")]
        MonsterTruck,

        [DataName("train")]
        Train,

        [DataName("trailer")]
        Trailer
    }
}
