using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SimpleCameraSetting
{
    /*
    [HarmonyPatch]
    public static class CanHighlightAnyPatch
    {
        [HarmonyPatch(typeof(SilhouetteUtility), nameof(SilhouetteUtility.CanHighlightAny))]
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
    */
    [HarmonyPatch]
    public static class GetAlphaPatch
    {
        static float lastCachedAlpha;
        static int lastCachedAlphaFrame;
        [HarmonyPatch(typeof(SilhouetteUtility), "GetAlpha")]
        [HarmonyPrefix]
        public static bool Prefix(ref float __result)
        {
            if (SimpleCameraModSetting.modSetting.silhouetteDistance == 100f) return true;
            if (lastCachedAlphaFrame == RealTime.frameCount)
            {
                __result = lastCachedAlpha;
                return false;
            }
            lastCachedAlphaFrame = RealTime.frameCount;
            int num1 = Prefs.HighlightStyleMode == HighlightStyleMode.Silhouettes ? 1 : 0;
            CameraDriver cameraDriver = Find.CameraDriver;
            float num2 = Mathf.Clamp01(Mathf.InverseLerp(SimpleCameraModSetting.modSetting.silhouetteDistance * 0.849999964f, SimpleCameraModSetting.modSetting.silhouetteDistance, cameraDriver.ZoomRootSize));
            __result= num1 != 0 ? (lastCachedAlpha = 0.9f * num2) : (lastCachedAlpha = 0.75f * num2);
            return false;
        
        }
    }
    
    [HarmonyPatch]
    public static class CanHighlightAnyPatch
    {
        [HarmonyPatch(typeof(SilhouetteUtility), nameof(SilhouetteUtility.CanHighlightAny))]
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            if (SimpleCameraModSetting.modSetting.silhouetteDistance == 100f) return true;
            if (Prefs.DotHighlightDisplayMode == DotHighlightDisplayMode.None)
            {
                __result = false;
                return false;
            }
            CameraDriver cameraDriver = Find.CameraDriver;
            __result = (double)cameraDriver.ZoomRootSize >= SimpleCameraModSetting.modSetting.silhouetteDistance * 0.89999997615814209;
            //Log.Message("res:"+__result);
            return false; // Prevent the original method from executing
        }
    }
    /*
    [HarmonyPatch(typeof(SilhouetteUtility))]
    public static class SilhouetteUtility_Patch
    {
        [HarmonyTranspiler]
        [HarmonyPatch("GetAlpha")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = new List<CodeInstruction>(instructions);
            var codesToPatch = new[]
            {
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Verse.FloatRange), "max")),
                new CodeInstruction(OpCodes.Ldc_R4, 0.849999964f),
                new CodeInstruction(OpCodes.Mul)
            };

            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList.Skip(i).Take(codesToPatch.Length).SequenceEqual(codesToPatch))
                {
                    instructionList[i + 1].operand = 60f; // sizeRange.max 값을 60으로 변경
                    i += codesToPatch.Length - 1; // 다음 코드로 건너뛰기
                }
            }

            return instructionList.AsEnumerable();
        }
    }*/
}
