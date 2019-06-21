/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DataNameAttribute : Attribute
    {
        public string Name { get; set; }

        public DataNameAttribute(string name)
        {
            Name = name;
        }
    }
}
