using System;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public abstract class Section
    {
        public SectionType Type = SectionType.None;
        public UInt32 Size;
        public UInt32 Version;
        public ExtendedSection Parent;

        public override string ToString() // helpful for debugger
        {
            return Type.ToString();
        }
    }
}
