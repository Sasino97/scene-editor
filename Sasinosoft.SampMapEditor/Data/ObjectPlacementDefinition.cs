/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.Data
{
    public class ObjectPlacementDefinition
    {
        public int Id { get; set; }
        public Point3D Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int Interior { get; set; }
        public int LODIndex { get; set; }
    }
}
