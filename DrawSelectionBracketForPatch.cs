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
    //폰을 따라가고 있을 때 테두리를 지워줌
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
