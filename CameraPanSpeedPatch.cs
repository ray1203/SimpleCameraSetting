using HarmonyLib;
using Verse;

namespace SimpleCameraSetting
{
    
    [HarmonyPatch]
    public static class CameraPanSpeedPatch
    {
        private static float desiredSize;
        private static float desiredSizeBefore=0f;
        [HarmonyPatch(typeof(CameraDriver),nameof(CameraDriver.CameraDriverOnGUI))]
        [HarmonyPrefix]
        public static bool Prefix(CameraDriver __instance)
        {
            desiredSize = Refs.desiredSize.Invoke(__instance);
            if (desiredSizeBefore != desiredSize)
            {
                if (desiredSize < 1f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_1;
                else if (desiredSize < 3f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_3;
                else if (desiredSize < 5f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_5;
                else if (desiredSize < 10f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_10;
                else if (desiredSize < 20f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_20;
                else if (desiredSize < 40f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_40;
                else if (desiredSize < 60f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_60;
                else __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_100;
            }
            desiredSizeBefore = desiredSize;
            return true;
        }
    }
}
