/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.IDE
{
    public enum IDEFlags : UInt32
    {
        IsRoad                  = 0x1,
        Unknown1                = 0x2,
        DrawLast                = 0x4,
        Additive                = 0x8,
        Unknown2                = 0x10,
        Animated                = 0x20,
        NoZBufferWrite          = 0x40,
        DontReceiveShadows      = 0x80,
        Unknown3                = 0x100,
        IsGlassType1            = 0x200,
        IsGlassType2            = 0x400,
        IsGarageDoor            = 0x800,
        IsDamageable            = 0x1000,
        IsTree                  = 0x2000,
        IsPalm                  = 0x4000,
        DoesNotCollideWithFlyer = 0x8000,
        Unknown4                = 0x10000,
        Unknown5                = 0x20000,
        Unknown6                = 0x40000,
        Unknown7                = 0x80000,
        IsTag                   = 0x100000,
        DisableBackfaceCulling  = 0x200000,
        IsBreakableStatue       = 0x400000
    }
}
