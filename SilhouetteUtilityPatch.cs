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
    [HarmonyPatch]
    public static class GetAlphaPatch
    {
        static float lastCachedAlpha;
        static int lastCachedAlphaFrame;
        //축소가 많이 될 경우 폰을 실루엣으로 표시하는 코드
        //기준을 모드 세팅의 값을 참고하도록 바꿈
        //만약 100일 경우 기존 코드를 사용함
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
    [HarmonyPatch]
    public static class AdjustScalePatch
    {
        [HarmonyPatch (typeof(SilhouetteUtility), nameof(SilhouetteUtility.AdjustScale))]
        [HarmonyPrefix]
        public static bool Prefix(ref float __result)
        {
            __result = 100f;
            return false;
        }
    }
}
