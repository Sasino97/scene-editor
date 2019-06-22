/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using Sasinosoft.SampMapEditor.Utils;

namespace Sasinosoft.SampMapEditor.Pedestrians
{
    public static class PedestrianUtils
    {
        public static PedestrianType PedestrianTypeFromString(string pedestrianTypeString)
        {
            foreach (PedestrianType pedestrianType in Enum.GetValues(typeof(PedestrianType)))
            {
                var memberInfo = typeof(PedestrianType).GetMember(pedestrianType.ToString());
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DataNameAttribute), false);

                if (attributes.Length < 1)
                    continue;

                var typeString = (attributes[0] as DataNameAttribute).Name;

                if (typeString == pedestrianTypeString)
                    return pedestrianType;
            }
            return PedestrianType.Undefined;
        }
    }
}
