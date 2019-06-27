/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System.Windows;
using System.Windows.Media.Animation;

namespace Sasinosoft.SampMapEditor
{
    public partial class App : Application
    {
        public App()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 60 }
            );
        }
    }
}
