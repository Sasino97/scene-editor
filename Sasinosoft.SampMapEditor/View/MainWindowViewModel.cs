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
