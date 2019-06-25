/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public class MaterialInfo
    {
        private static readonly Color selectionColor = Color.FromArgb(100, 30, 255, 30);
        public MaterialGroup MaterialGroup;
        public DiffuseMaterial SelectionMaterial;

        public void SetIsSelected(bool isSelected)
        {
            if (isSelected)
            {
                SelectionMaterial = new DiffuseMaterial(new SolidColorBrush(selectionColor));
                MaterialGroup.Children.Add(SelectionMaterial);
            }
            else
            {
                if (SelectionMaterial != null)
                    MaterialGroup.Children.Remove(SelectionMaterial);
            }
        }
    }
}
