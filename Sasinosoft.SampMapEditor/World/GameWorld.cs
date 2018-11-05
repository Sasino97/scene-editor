/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Sasinosoft.SampMapEditor.World
{
    public class GameWorld
    {
        /// <summary>
        /// Gets or set a value representing the maximum distance at which objects 
        /// will be loaded in memory and rendered.
        /// </summary>
        public double StreamDistance { get; set; }

        //
        private List<GameObject> gameObjects = new List<GameObject>();
        private Viewport3D viewport;

        //
        public GameWorld(Viewport3D viewport)
        {
            this.viewport = viewport;
        }

        //
        public void Update()
        {
            // Updates the world by streaming only the necessary objects by
            // adding them to the viewport, and removing the unnecessary ones
            // This should act as a garbage collector as well, by deleting 
            // references to the objects which are not being streamed since
            // a long time
        }
    }
}
