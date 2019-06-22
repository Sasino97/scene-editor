/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.RenderWare.Dff;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public class RenderWareModel : ModelVisual3D
    {
        private struct MaterialInfo
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

        private Dictionary<string, List<MaterialInfo>> MaterialGroupDictionary = new Dictionary<string, List<MaterialInfo>>();

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    foreach (List<MaterialInfo> materialInfoList in MaterialGroupDictionary.Values)
                    {
                        foreach (MaterialInfo materialInfo in materialInfoList)
                        {
                            materialInfo.SetIsSelected(value);
                        }
                    }
                }
            }
        }

        public RenderWareModel() : base() { }
        public RenderWareModel(ExtendedSection clump) : base()
        {
            // populate
            var model3dGroup = new Model3DGroup();
            Content = model3dGroup;
            MaterialGroupDictionary.Clear();
            //

            foreach (Section geometryList in clump.GetChildren(SectionType.RwGeometryList))
            {
                foreach (Section geometry in ((ExtendedSection)geometryList).GetChildren(SectionType.RwGeometry))
                {
                    var geometryInfo = (GeometryDataSection)((ExtendedSection)geometry).GetChild(0);
                    var model3d = new GeometryModel3D();
                    var meshGeometry3d = new MeshGeometry3D();
                    model3d.Geometry = meshGeometry3d;
                    model3dGroup.Children.Add(model3d);

                    //
                    foreach (var vertex in geometryInfo.Vertices)
                    {
                        meshGeometry3d.Positions.Add(new Point3D(vertex.X, vertex.Z, vertex.Y));
                    }
                    foreach (var normal in geometryInfo.Normals)
                    {
                        meshGeometry3d.Normals.Add(new Vector3D(normal.X, normal.Z, normal.Y));
                    }
                    foreach (var uvcoord in geometryInfo.VertexUVs)
                    {
                        meshGeometry3d.TextureCoordinates.Add(new Point(uvcoord.U, uvcoord.V));
                    }
                    foreach (var triangle in geometryInfo.Triangles)
                    {
                        meshGeometry3d.TriangleIndices.Add(triangle.Vertex2);
                        meshGeometry3d.TriangleIndices.Add(triangle.Vertex1);
                        meshGeometry3d.TriangleIndices.Add(triangle.Vertex3);
                    }

                    var matGroup = new MaterialGroup();
                    model3d.Material = matGroup;
                    matGroup.Children.Add(new DiffuseMaterial(Brushes.White));

                    foreach (Section materialList in ((ExtendedSection)geometry).GetChildren(SectionType.RwMaterialList))
                    {
                        foreach (Section material in ((ExtendedSection)materialList).GetChildren(SectionType.RwMaterial))
                        {
                            foreach (Section texture in ((ExtendedSection)material).GetChildren(SectionType.RwTexture))
                            {
                                var textureInfo = (StringDataSection)((ExtendedSection)texture).GetChild(1);

                                if (!MaterialGroupDictionary.ContainsKey(textureInfo.String))
                                {
                                    var list = new List<MaterialInfo>
                                        { new MaterialInfo() { MaterialGroup = matGroup } };
                                    MaterialGroupDictionary.Add(textureInfo.String, list);
                                }
                                else
                                {
                                    var list = MaterialGroupDictionary[textureInfo.String];
                                    list.Add(new MaterialInfo() { MaterialGroup = matGroup });
                                }
                            }
                        }
                    }

                }
            }
            model3dGroup.Children.Add(new AmbientLight());
        }
        
        public void SetTextureData(RenderWareTextureDictionary txd)
        {
            if (txd == null)
                return;

            foreach(string name in MaterialGroupDictionary.Keys)
            {
                List<MaterialInfo> materialInfoList = MaterialGroupDictionary[name];

                if (txd.MaterialDictionary.TryGetValue(name, out Material material))
                {
                    foreach (MaterialInfo info in materialInfoList)
                    {
                        info.MaterialGroup.Children.Add(material);
                    }
                }
            }
        }
    }
}
