/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using Sasinosoft.SampMapEditor.Utils;

namespace Sasinosoft.SampMapEditor.Vehicles
{
    public static class VehicleUtils
    {
        public static VehicleType VehicleTypeFromString(string vehicleTypeString)
        {
            foreach (VehicleType vehicleType in Enum.GetValues(typeof(VehicleType)))
            {
                var memberInfo = typeof(VehicleType).GetMember(vehicleType.ToString());
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DataNameAttribute), false);

                if (attributes.Length < 1)
                    continue;

                var typeString = (attributes[0] as DataNameAttribute).Name;

                if (typeString == vehicleTypeString)
                    return vehicleType;
            }
            return VehicleType.Undefined;
        }

        public static VehicleClass VehicleClassFromString(string vehicleClassString)
        {
            foreach (VehicleClass vehicleClass in Enum.GetValues(typeof(VehicleClass)))
            {
                var memberInfo = typeof(VehicleClass).GetMember(vehicleClass.ToString());
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DataNameAttribute), false);

                if (attributes.Length < 1)
                    continue;

                var typeString = (attributes[0] as DataNameAttribute).Name;

                if (typeString == vehicleClassString)
                    return vehicleClass;
            }
            return VehicleClass.Undefined;
        }
    }
}
