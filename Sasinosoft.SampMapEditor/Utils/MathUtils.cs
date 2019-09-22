/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.Utils
{
    public static class MathUtils
    {
        private const double Epsilon = 0.00001;

        // Credits to Nominal Animal on StackExchange
        // https://math.stackexchange.com/questions/1477926/quaternion-to-euler-with-some-properties
        // Also thanks to IllidanS4 for posting the question
        public static Point3D QuaternionToEuler(Quaternion q)
        {
            double w = q.W;
            double x = q.X;
            double y = q.Y;
            double z = q.Z;
            double t = 2 * (y * z - x * w);
            double xa = 0, ya = 0, za = 0;

            if (t >= 1 - Epsilon)
            {
                xa = Math.PI / 2;
                ya = -Math.Atan2(y, w);
                za = -Math.Atan2(z, w);
            }
            else if (-t >= 1 - Epsilon)
            {
                xa = -Math.PI / 2;
                ya = -Math.Atan2(y, w);
                za = -Math.Atan2(z, w);
            }
            else
            {
                xa = Math.Asin(t);
                ya = -Math.Atan2(x * z + y * w, 0.5 - x * x - y * y);
                za = -Math.Atan2(x * y + z * w, 0.5 - x * x - z * z);
            }
            return new Point3D(xa * 180 / Math.PI + 90, ya * 180 / Math.PI, za * 180 / Math.PI);
        }
        /*
        public static Quaternion EulerToQuaternion(Point3D a)
        {


            //cx = cos(-0.5 * xa)
            //sx = sin(-0.5 * xa)
            //cy = cos(-0.5 * ya)
            //sy = sin(-0.5 * ya)
            //cz = cos(-0.5 * za)
            //sz = sin(-0.5 * za)
            //w = cx * cy * cz + sx * sy * sz
            //i = cx * sy * sz + sx * cy * cz
            //j = cx * sy * cz - sx * cy * sz
            //k = cx * cy * sz - sx * sy * cz
            //return (w, i, j, k)
        }
        */
    }
}
