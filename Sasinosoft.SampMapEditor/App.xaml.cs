/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Sasinosoft.SampMapEditor
{
    public partial class App : Application
    {
        public App()
        {
            // Very important. Miss these lines, and the decimal parsing might behave differently
            // according to the geographical location of the computer running this app.
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
