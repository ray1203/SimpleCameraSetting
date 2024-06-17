using HarmonyLib;
using RimWorld;
using Verse;

namespace SimpleCameraSetting
{
    //폰을 따라가고 있을 때 테두리를 지워줌
    [HarmonyPatch]
    public static class PawnNamePatch
    {
        [HarmonyPatch(typeof(Pawn), nameof(Pawn.DrawGUIOverlay))]
        [HarmonyPrefix]
        static bool Prefix()
        {
            if(SimpleCameraModSetting.modSetting.hidePawnName&&
                (double)Find.CameraDriver.ZoomRootSize >= SimpleCameraModSetting.modSetting.silhouetteDistance * 0.89999997615814209)return false;
            return true;
        }
    }
}
