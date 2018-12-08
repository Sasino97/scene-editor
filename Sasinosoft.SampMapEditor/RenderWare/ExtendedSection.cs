/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public class ExtendedSection : Section
    {
        public int CreationOffset { get; set; }
        private List<Section> children = new List<Section>();

        public void AddChild(Section section)
        {
            section.Parent = this;
            children.Add(section);
        }

        public Section GetChild(int index)
        {
            return children[index];
        }

        public IEnumerable<Section> GetChildren()
        {
            foreach (Section s in children)
                yield return s;
        }

        public IEnumerable<Section> GetChildren(SectionType type)
        {
            foreach (Section s in children)
            {
                if(s.Type == type)
                    yield return s;
            }
        }

        public string GetTreeString()
        {
            return GetChildrenTree();
        }

        private string GetChildrenTree(int level = 0)
        {
            string ret = "";

            for (int i = 0; i < level; i++)
                ret += "  ";

            ret += Type.ToString() + "\n";

            foreach (Section section in children)
            {
                if (section.GetType() == typeof(ExtendedSection))
                {
                    ret += ((ExtendedSection)section).GetChildrenTree(level + 1);
                }
                else
                {
                    for (int i = 0; i < level+1; i++)
                        ret += "  ";
                    ret += section.Type.ToString();

                    if (section.GetType() == typeof(DataSection))
                        if (((DataSection)section).IsDamaged)
                            ret += " [Damaged]";

                    ret += "\n";
                }
            }
            return ret;
        }
    }
}
