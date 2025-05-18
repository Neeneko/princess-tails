using RimWorld;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;
using ReviaRace.Genes;
using HarmonyLib;
using Verse.AI;
using System.Collections;

namespace PrincessTails
{

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Type patchType = typeof(HarmonyPatches);

        //private static bool showAllHediffs = true;
        private static FieldInfo showAllHediffs = null;

        static HarmonyPatches()
        {
            var harmony = new Harmony("Princess.Tails");
            Verse.Log.Warning("[MYRK]\tTailPatch Setup");
            //var foo = typeof(HealthCardUtility).GetMethod("VisibleHediffs", BindingFlags.NonPublic | BindingFlags.Static);
            //Verse.Log.Warning("[MYRK] Method : " + foo);
            showAllHediffs = typeof(HealthCardUtility).GetField("showAllHediffs", BindingFlags.NonPublic | BindingFlags.Static);
            harmony.Patch(typeof(HealthCardUtility).GetMethod("VisibleHediffs", BindingFlags.NonPublic | BindingFlags.Static), postfix: new HarmonyMethod(patchType, nameof(VisibleHediffs)));
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //harmony.Patch(AccessTools.Method(typeof(HealthCardUtility),"HealthCardUtility.VisibleHediffs"), postfix: new HarmonyMethod(patchType, nameof(VisibleHediffs)));
        }

        //private static IEnumerable<Hediff> VisibleHediffs(Pawn pawn, bool showBloodLoss)
        private static IEnumerable<Hediff> VisibleHediffs(IEnumerable<Hediff> __result)
        {
            //HealthCardUtility.
            //Verse.Log.Warning("[MYRK]\tVisibleHediffs");
            foreach (Hediff item in __result)
            {
                bool a = item is Hediff_HiddenPart;
                bool b = item.GetType().IsInstanceOfType(typeof(Hediff_HiddenPart));
                //Verse.Log.Warning("[MYRK] A[ " + a +"] B ["+ b + "] item " + item + " type " + item.GetType() + " vs " + typeof(Hediff_HiddenPart));
                Type objType = showAllHediffs.GetType();
                bool value = (bool)showAllHediffs.GetValue(objType);
                if (!value && item.GetType() == typeof(Hediff_HiddenPart))
                {
                    //Verse.Log.Warning("[MYRK} hide!");
                    continue;
                }
                else
                {
                    yield return item;
                }
            }
            yield break;
        }
    }


    public class Hediff_HiddenPart : Hediff_MissingPart
    {

    }

    [DefOf]
    public static class TailPatchDefs
    {
        public static HediffDef Hediff_HiddenPart;
        //public static HediffDef Hediff_MissingTail;
    }



    public class Gene_TailTest : ReviaTailGene
    {
        public override void PostAdd()
        {
            Verse.Log.Warning("[MYRK] PostAdd");
            base.PostAdd();
            if (!pawn.kindDef.race.defName.ToUpper().Contains("SNAKE"))
            {
               // var toRemove = new List<BodyPartRecord> { };
                foreach (BodyPartRecord record in pawn.RaceProps.body.AllParts)
                {
                    //Verse.Log.Warning("[MYRK] [" + record.Label + "]");
                    if (record.Label.ToUpper().Contains("TAIL") && !record.Label.ToUpper().Contains("REVIA"))
                    {
                        Verse.Log.Warning("[MYRK]\t[" + record.Label + "]");
                        //pawn.health.AddHediff(HediffDefOf.)
                        Hediff hediff = pawn.health.AddHediff(TailPatchDefs.Hediff_HiddenPart, record);
                        hediff.Severity =  1;
                        Verse.Log.Warning("[MYRK]\tHediff [" + hediff + "] Visible [" +hediff.Visible + "]");
                        //pawn.health.AddHediff(HediffDefOf.MissingBodyPart, record);
                        //toRemove.Add(record);

                        //
                    }
                }
                //var fxn = typeof(HealthCardUtility).GetMethod("VisibleHediffs", BindingFlags.NonPublic | BindingFlags.Static);
                //IEnumerable<Hediff> result = (IEnumerable<Hediff>)fxn.Invoke(null, new object[] { pawn, false });
                //foreach (IEnumerable<Hediff> foo in result)
                //{
                //    Verse.Log.Warning("[MYRK]");
                //}


                //foreach (BodyPartRecord record in toRemove)
                //{ 
                //    pawn.RaceProps.body.AllParts.Remove(record);
                // }
                //HealthCardUtility.showAllHediffs
           }

            //if (pawn.IsKurin())
            //{
            //Verse.Log.Warning("[MYRK] Is Kurin");
            //if (!pawn.health.hediffSet.HasHediff(OutlandGenesDefOf.Outland_XenotypeAscender))
            //{
            //    Hediff_XenotypeAscender hediff = (Hediff_XenotypeAscender)HediffMaker.MakeHediff(OutlandGenesDefOf.Outland_XenotypeAscender, pawn, null);
            //    pawn.health.AddHediff(hediff);
            //}
        }

        public override void PostRemove()
        {
            base.PostRemove();
            Verse.Log.Warning("[MYRK] PostRemove");
            //if (pawn.health.hediffSet.HasHediff(OutlandGenesDefOf.Outland_XenotypeAscender))
            //{
            //    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(OutlandGenesDefOf.Outland_XenotypeAscender));
            //}
        }
    }
}
