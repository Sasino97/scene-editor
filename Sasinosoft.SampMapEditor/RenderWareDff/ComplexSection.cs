using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class ComplexSection : Section
    {
        public List<Section> Children { get; private set; } = new List<Section>();
    }
}
