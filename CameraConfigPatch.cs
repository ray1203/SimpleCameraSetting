using Verse;
using HarmonyLib;

namespace SimpleCameraSetting
{
    [HarmonyPatch]
    public static class CameraConfigPatch
    {
        public static void ConfigPatch()
        {

            if (Current.CameraDriver != null)
            {
                Current.CameraDriver.config.sizeRange.min = SimpleCameraModSetting.cameraSetting.sizeRange.min;
                Current.CameraDriver.config.sizeRange.max = SimpleCameraModSetting.cameraSetting.sizeRange.max;
                Current.CameraDriver.config.zoomSpeed = SimpleCameraModSetting.cameraSetting.zoomSpeed;
                Current.CameraDriver.config.smoothZoom = SimpleCameraModSetting.cameraSetting.smoothZoom;
                //Current.CameraDriver.config.followSelected = SettingPatch.cameraSetting.followSelected;
            }
        }
        [HarmonyPatch(typeof(Game), nameof(Game.InitNewGame))]
        [HarmonyPrefix]
        public static void PreFix()
        {
            CameraConfigPatch.ConfigPatch();
            Log.Message("CameraSetting Applied");
            //Log.Message(Current.CameraDriver.config);
        }

        [HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
        [HarmonyPrefix]
        public static void LoadGamePreFix()
        {
            CameraConfigPatch.ConfigPatch();
            Log.Message("CameraSetting Applied");
            //Log.Message(Current.CameraDriver.config);
        }

        [HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.CurrentZoom), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CurrentZoomPreFix(CameraDriver __instance, ref CameraZoomRange __result)
        {
            float rangeMin = __instance.config.sizeRange.min;
            float rangeMax = __instance.config.sizeRange.max;
            if (rangeMin < 11) rangeMin = 11;
            if (rangeMax > 60) rangeMax = 60;
            if ((double)__instance.ZoomRootSize < rangeMin + 1.0)
            {
                __result = CameraZoomRange.Closest;
                return false;
            }
            if ((double)__instance.ZoomRootSize < (double)rangeMax * 0.23000000417232513)
            {
                __result = CameraZoomRange.Close;
                return false;
            }
            if ((double)__instance.ZoomRootSize < (double)rangeMax * 0.699999988079071)
            {
                __result = CameraZoomRange.Middle;
                return false;
            }
            __result = ((double)__instance.ZoomRootSize < (double)rangeMax * 0.949999988079071) ? CameraZoomRange.Far : CameraZoomRange.Furthest;
            return false;
        }
    }
}
