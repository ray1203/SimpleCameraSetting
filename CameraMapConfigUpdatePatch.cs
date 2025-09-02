
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace SimpleCameraSetting
{
    //CameraFollowing 기능 관련
    //메시지 띄우기, 다른 림 Follow 시 부드럽게 이동 등
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
