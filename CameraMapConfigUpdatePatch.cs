
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace SimpleCameraSetting
{
    [HarmonyPatch]
    public static class CameraMapConfigUpdatePatch
    {
        static List<Pawn> lastSelectedPawns=new List<Pawn>();
        [HarmonyPatch(typeof(CameraMapConfig), nameof(CameraMapConfig.ConfigFixedUpdate_60))]
        [HarmonyPrefix]
        public static bool Prefix(ref CameraMapConfig __instance, ref Vector3 rootPos, ref Vector3 velocity)
        {
            if (__instance.followSelected)
            {
                List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
                //폰을 선택하고 있지 않을 경우 폰 따라가기 기능을 자동으로 비활성화함
                //modSetting.autoOffFollow가 켜져있어야만 작동
                //added code
                if (selectedPawns.Empty())
                {
                    if (SimpleCameraModSetting.modSetting.autoOffFollow)
                    {
                        Current.CameraDriver.config.followSelected = false;
                        Messages.Message("Camera Following " + (Current.CameraDriver.config.followSelected ? "On" : "Off"), new MessageTypeDef());
                    } 
                    else
                    {
                        selectedPawns = lastSelectedPawns;
                    }
                }else lastSelectedPawns = new List<Pawn>(selectedPawns);
                //code end
                if (selectedPawns.Count > 0)
                {
                    Vector3 zero = Vector3.zero;
                    int num = 0;
                    foreach (Pawn t in selectedPawns)
                    {
                        if (t.MapHeld == Find.CurrentMap)
                        {
                            zero += t.TrueCenter();
                            ++num;
                        }
                    }
                    if (num > 0)
                    {
                        Vector3 target = zero / (float)num;
                        target.y = rootPos.y;
                        //original code
                        //rootPos = Vector3.MoveTowards(rootPos, target, 0.02f * Mathf.Max(Find.TickManager.TickRateMultiplier, 1f) * this.moveSpeedScale);
                        //Messages.Message("" + target, new MessageTypeDef());
                        
                        //카메라가 폰을 따라가는 방식을 즉시 이동으로 바꿈
                        //added code
                        Find.CameraDriver.JumpToCurrentMapLoc(target);
                        //code end
                    }
                }
            }
            if ((double)__instance.autoPanSpeed <= 0.0 || Find.TickManager.CurTimeSpeed == TimeSpeed.Paused && !__instance.autoPanWhilePaused)
                return false;
            velocity.x = Mathf.Cos(__instance.autoPanAngle) * __instance.autoPanSpeed;
            velocity.z = Mathf.Sin(__instance.autoPanAngle) * __instance.autoPanSpeed;
            return false;
        }

    }
}
