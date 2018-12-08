/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    [Flags]
    public enum GeometryDataFlags : UInt16
    {
        None = 0x00,
        RwObjectVertexTristrip = 0x01,
        RwObjectVertexPositions = 0x02,
        RwObjectVertexUv = 0x04,
        RwObjectVertexColor = 0x08,
        RwObjectVertexNormal = 0x10,
        RwObjectVertexLight = 0x20,
        RwObjectVertexModulate = 0x40,
        RwObjectVertexTextured = 0x80
    }
}
