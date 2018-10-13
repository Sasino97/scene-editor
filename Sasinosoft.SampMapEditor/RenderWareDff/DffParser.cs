using System.IO;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public static class DffParser
    {
        public static DffModel Parse(Stream stream)
        {

            return null;
        }

        public static DffModel Parse(string fileName)
        {
            return Parse(new FileStream(fileName, FileMode.Open));
        }
    }
}
