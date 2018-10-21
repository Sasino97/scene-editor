using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Sasinosoft.SampMapEditor.Utils
{
    public static class MouseUtils
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Point
        {
            public int X;
            public int Y;
        };

        public static Point GetPosition(Visual relativeTo)
        {
            return relativeTo.PointFromScreen(GetPosition());
        }

        public static Point GetPosition()
        {
            Win32Point p = new Win32Point();
            GetCursorPos(ref p);
            return new Point(p.X, p.Y);
        }

        public static void SetPosition(Point p)
        {
            SetPosition(p.X, p.Y);
        }

        public static void SetPosition(double x, double y)
        {
            SetCursorPos((int)x, (int)y);
        }
    }
}
