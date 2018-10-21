using Sasinosoft.SampMapEditor.RenderWare.Dff;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public class RenderWareModel : ModelVisual3D
    {
        private static readonly Color selectionColor = Color.FromArgb(100, 30, 255, 30);

        private ExtendedSection clump;
        private Dictionary<string, List<MaterialGroup>> MaterialGroupDictionary = new Dictionary<string, List<MaterialGroup>>();
        private bool isSelected = false;

        public RenderWareModel() : base() { }
        public RenderWareModel(ExtendedSection clump) : base()
        {
            this.clump = clump;
            SetModelData();
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if(isSelected != value)
                {
                    isSelected = value;
                    foreach (List<MaterialGroup> groupList in MaterialGroupDictionary.Values)
                    {
                        foreach(MaterialGroup group in groupList)
                        {
                            if (value)
                                group.Children.Add(new DiffuseMaterial(new SolidColorBrush(selectionColor)));
                            else
                                group.Children.RemoveAt(group.Children.Count - 1);
                        }
                    }
                }
            }
        }
        
        private void SetModelData()
        {
            // populate
            var model3dGroup = new Model3DGroup();
            Content = model3dGroup;
            MaterialGroupDictionary.Clear();
            //

            foreach (Section geometryList in clump.GetChildren(SectionType.RwGeometryList))
            {
                foreach(Section geometry in ((ExtendedSection)geometryList).GetChildren(SectionType.RwGeometry))
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

                                if(!MaterialGroupDictionary.ContainsKey(textureInfo.String))
                                {
                                    var list = new List<MaterialGroup>
                                    {
                                        matGroup
                                    };
                                    MaterialGroupDictionary.Add(textureInfo.String, list);
                                }
                                else
                                {
                                    var list = MaterialGroupDictionary[textureInfo.String];
                                    list.Add(matGroup);
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
            foreach(string name in MaterialGroupDictionary.Keys)
            {
                List<MaterialGroup> groupList = MaterialGroupDictionary[name];

                if (txd.MaterialDictionary.TryGetValue(name, out Material material))
                {
                    foreach (MaterialGroup group in groupList)
                    {
                        group.Children.Add(material);
                    }
                }
            }
        }
    }
}
