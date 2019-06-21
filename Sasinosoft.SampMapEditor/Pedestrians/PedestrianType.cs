/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.Utils;

namespace Sasinosoft.SampMapEditor.Pedestrians
{
    /// <summary>
    /// Pedestrian types.
    /// </summary>
    public enum PedestrianType
    {
        [DataName("PLAYER1")]
        Player1,

        [DataName("PLAYER2")]
        Player2,

        [DataName("PLAYER_NETWORK")]
        PlayerNetwork,

        [DataName("PLAYER_UNUSED")]
        PlayerUnused,

        [DataName("CIVMALE")]
        CivilianMale,

        [DataName("CIVFEMALE")]
        CivilianFemale,

        [DataName("COP")]
        Cop,

        [DataName("GANG1")]
        Ballas,

        [DataName("GANG2")]
        Grove,

        [DataName("GANG3")]
        Vagos,

        [DataName("GANG4")]
        Rifa,

        [DataName("GANG5")]
        DaNangBoys,

        [DataName("GANG6")]
        Mafia,

        [DataName("GANG7")]
        Triads,

        [DataName("GANG8")]
        Aztecas,

        [DataName("GANG9")]
        UnusedGang1,

        [DataName("GANG10")]
        UnusedGang2,

        [DataName("DEALER")]
        DrugDealer,

        [DataName("EMERGENCY")]
        Emergency,

        [DataName("FIREMAN")]
        Fireman,

        [DataName("CRIMINAL")]
        Criminal,

        [DataName("BUM")]
        Unused,

        [DataName("SPECIAL")]
        Special,

        [DataName("PROSTITUTE")]
        Prostitute,

        [DataName("MISSION1")]
        Mission1,

        [DataName("MISSION2")]
        Mission2,

        [DataName("MISSION3")]
        Mission3,

        [DataName("MISSION4")]
        Mission4,

        [DataName("MISSION5")]
        Mission5,

        [DataName("MISSION6")]
        Mission6,

        [DataName("MISSION7")]
        Mission7,

        [DataName("MISSION8")]
        Mission8
    }
}
