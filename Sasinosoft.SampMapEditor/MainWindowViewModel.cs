using System;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor
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

        // Camera information //

        private Point3D cameraPosition = new Point3D(0, 0, 5);
        public Point3D CameraPosition
        {
            get { return cameraPosition; }
            set
            {
                if (cameraPosition != value)
                {
                    cameraPosition = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Transform3D cameraTransform = new Transform3DGroup();
        public Transform3D CameraTransform
        {
            get { return cameraTransform; }
            set
            {
                if (cameraTransform != value)
                {
                    cameraTransform = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Vector3D cameraLookDirection = new Vector3D(0, 0, -1);
        public Vector3D CameraLookDirection
        {
            get { return cameraLookDirection; }
            set
            {
                if (cameraLookDirection != value)
                {
                    cameraLookDirection = value;
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

        // private variables
        private double cameraPitch;
        private double cameraYaw;

        public MainWindowViewModel()
        {
        }

        public void RotateCamera(double yaw, double pitch)
        {
            cameraYaw -= yaw;
            cameraPitch += pitch;

            if (cameraPitch < -90)
                cameraPitch = -90;

            if (cameraPitch > 90)
                cameraPitch = 90;

            CameraLookDirection = new Vector3D(
                Math.Sin(cameraYaw) * Math.Cos(cameraPitch),
                Math.Sin(cameraPitch),
                Math.Cos(cameraYaw) * Math.Cos(cameraPitch)
            );
        }

        public void ZoomCamera(double amount)
        {
            CameraPosition = new Point3D(
                CameraPosition.X + (CameraLookDirection.X * amount),
                CameraPosition.Y + (CameraLookDirection.Y * amount),
                CameraPosition.Z + (CameraLookDirection.Z * amount)
            );
        }
    }
}
