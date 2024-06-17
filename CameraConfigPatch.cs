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
                Current.CameraDriver.config.sizeRange.min = SimpleCameraModSetting.modSetting.sizeRange.min;
                if (Current.CameraDriver.config.sizeRange.min == 0) Current.CameraDriver.config.sizeRange.min = 0.001f;
                Current.CameraDriver.config.sizeRange.max = SimpleCameraModSetting.modSetting.sizeRange.max;
                Current.CameraDriver.config.zoomSpeed = SimpleCameraModSetting.modSetting.zoomSpeed;
                Current.CameraDriver.config.smoothZoom = SimpleCameraModSetting.modSetting.smoothZoom;
                //Current.CameraDriver.config.followSelected = SettingPatch.cameraSetting.followSelected;
#if DEBUG
                Log.Message("CameraSetting Applied");
#endif
            }
        }
        [HarmonyPatch(typeof(Game), nameof(Game.InitNewGame))]
        [HarmonyPrefix] 
        public static void PreFix()
        {
            CameraConfigPatch.ConfigPatch();
            //Log.Message(Current.CameraDriver.config);
        }

        [HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
        [HarmonyPrefix]
        public static void LoadGamePreFix()
        {
            CameraConfigPatch.ConfigPatch();
            //Log.Message(Current.CameraDriver.config);
        }

        [HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.CurrentZoom), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CurrentZoomPreFix(CameraDriver __instance, ref CameraZoomRange __result)
        {
            float rangeMin = SimpleCameraModSetting.modSetting.displayDistance;
            float rangeMax = __instance.config.sizeRange.max;
            if (rangeMax > 60) rangeMax = 60;
            if ((double)__instance.ZoomRootSize < rangeMin + 1.0)
            {
                //아마 Closest 말고 다른 요소는 사용하지 않음
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
