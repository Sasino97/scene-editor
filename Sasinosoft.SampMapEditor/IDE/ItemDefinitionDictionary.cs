/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.IDE
{
    public class ItemDefinitionDictionary
    {
        public Dictionary<int, ObjectDefinition> Objects = new Dictionary<int, ObjectDefinition>();
        public Dictionary<int, WeaponDefinition> Weapons = new Dictionary<int, WeaponDefinition>();
        public Dictionary<int, VehicleDefinition> Vehicles = new Dictionary<int, VehicleDefinition>();

        /// <summary>
        /// An Item Definition (IDE) Dictionary.
        /// </summary>
        public ItemDefinitionDictionary() { }
    }
}
