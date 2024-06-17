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
    [HarmonyPatch]
    public static class DrawSelectionBracketForPatch
    {
        [HarmonyPatch(typeof(SelectionDrawer),nameof(SelectionDrawer.DrawSelectionBracketFor))]
        [HarmonyPrefix]
        static bool Prefix()
        {
            if (Current.CameraDriver.config.followSelected) return false;

            return true;
        }
    }
}
