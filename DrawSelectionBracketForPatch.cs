using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SimpleCameraSetting
{
    //Following 중 림에 테두리가 생기면 안이뻐서 지워줌
    [HarmonyPatch]
    public static class DrawSelectionBracketForPatch
    {
        [HarmonyPatch(typeof(SelectionDrawer),nameof(SelectionDrawer.DrawSelectionBracketFor))]
        [HarmonyPrefix]
        static bool Prefix()
        {
            var mode = SimpleCameraModSetting.modSetting.bracketHideMode;
            if (mode == BracketHideMode.Always) return false;
            if (mode == BracketHideMode.WhenFollowing && Current.CameraDriver.config.followSelected) return false;

            return true;
        }
    }
}
