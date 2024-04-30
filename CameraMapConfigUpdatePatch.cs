
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
        [HarmonyPatch(typeof(CameraMapConfig), nameof(CameraMapConfig.ConfigFixedUpdate_60))]
        [HarmonyPrefix]
        public static bool Prefix( ref CameraMapConfig __instance,ref Vector3 rootPos,ref Vector3 velocity)
        {
            if (__instance.followSelected)
            {
                List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
                if (selectedPawns.Empty())
                {
                    Current.CameraDriver.config.followSelected = false;
                    Messages.Message("Camera Following " + (Current.CameraDriver.config.followSelected ? "On" : "Off"), new MessageTypeDef());


                }
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
                        Find.CameraDriver.JumpToCurrentMapLoc(target);
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
