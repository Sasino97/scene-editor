/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.Utils;
using System;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.View
{
    public class MainWindowViewModel : ViewModel
    {
        // Window information //
        private string title = "Sasinosoft Map Editor For SA-MP";
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged();
                }
            }
        }

        // Other information
        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                if(isReady != value)
                {
                    isReady = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
