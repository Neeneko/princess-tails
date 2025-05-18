using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PrincessTails
{
    public static class DebugActionsPrincess
    { 

        [DebugAction("Princess", "View render tree", false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ViewRenderTree(Pawn p)
        {
            Find.WindowStack.Add(new Dialog_DebugRenderTree(p));
        }
    }

}
