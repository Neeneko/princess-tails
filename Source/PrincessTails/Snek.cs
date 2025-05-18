using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PrincessTails
{

    [HarmonyPatch(typeof(PawnRenderNode_Body), "GraphicFor")]
    public static class Patch_PawnRenderNode_Body
    {
        [HarmonyPostfix]
        public static Verse.Graphic Postfix(Verse.Graphic __result, PawnRenderNode_Body __instance, Pawn pawn)
        {
            if (__result == null)
            {
                return null;
            }
            else if(pawn.story.furDef != null)
            {
                Shader shader = __instance.ShaderFor(pawn);
                return GraphicDatabase.Get<Graphic_Multi>(pawn.story.furDef.GetFurBodyGraphicPath(pawn), shader, Vector2.one, __instance.ColorFor(pawn));
            }
            else
            {
                return __result;
            }
        }
    }
}