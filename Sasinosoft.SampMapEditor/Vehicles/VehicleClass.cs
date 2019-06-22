/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.Utils;

namespace Sasinosoft.SampMapEditor.Vehicles
{
    /// <summary>
    /// Vehicle classes.
    /// </summary>
    public enum VehicleClass
    {
        [DataName(null)]
        Undefined,

        [DataName("normal")]
        Normal,

        [DataName("poorfamily")]
        PoorFamily,

        [DataName("richfamily")]
        RichFamily,

        [DataName("executive")]
        Executive,

        [DataName("worker")]
        Worker,

        [DataName("big")]
        Big,

        [DataName("taxi")]
        Taxi,

        [DataName("moped")]
        Moped,

        [DataName("motorbike")]
        Motorbike,

        [DataName("leisureboat")]
        LeisureBoat,

        [DataName("workerboat")]
        WorkerBoat,

        [DataName("bicycle")]
        Bicycle,

        [DataName("ignore")]
        Ignore
    }
}
