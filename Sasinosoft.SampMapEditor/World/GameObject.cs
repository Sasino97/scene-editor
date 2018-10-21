﻿using Sasinosoft.SampMapEditor.RenderWare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasinosoft.SampMapEditor.World
{
    public class GameObject
    {
        /// <summary>
        /// Gets or set the underlying 3D Model used in the rendering of this object.
        /// The Model is a ModelVisual3D, so it has a translation, rotation and scaling.
        /// </summary>
        public RenderWareModel Model { get; set; }

        /// <summary>
        /// Gets or sets a value which tells whether the object is part of the default 
        /// GTA:SA scenery. This is needed for the "RemoveBuilding" feature support.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets 
        /// </summary>
        public DateTime LastStreamTime { get; set; }
    }
}