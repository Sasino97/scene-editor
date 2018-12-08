using Sasinosoft.SampMapEditor.RenderWare;
using System;

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
        /// Gets a valure representing the last time this object passed the stream check. 
        /// This value is store so that objects which are not being streamed in a long time
        /// can be removed from memory completely, forcing to it be recreated again if needed.
        /// </summary>
        public DateTime LastStreamTime { get; set; }
    }
}
