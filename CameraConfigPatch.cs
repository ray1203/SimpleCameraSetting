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
        [HarmonyPostfix]
        public static void CurrentZoomPostFix(CameraDriver __instance, ref CameraZoomRange __result)
        {
            double rangeMin = SimpleCameraModSetting.modSetting.displayDistance;

            // Closest 외에는 사용하지 않는 것으로 보이기에 해당 조건일때만 Override
            // 아이템 수량 표시, 게이지 등이 Closest 가 아닐 경우 Skip함
            if (__instance.RootSize < rangeMin + 1.0)
            {
                __result = CameraZoomRange.Closest;
            }
        }
        //기존에는 Closest 외의 CameraZoomRange 기준을 바닐라와 동일하게 맞추기 위해서 Prefix하여서 rangeMax를 60으로 제한
        //하지만 프레임당 호출 횟수가 비 정상적(13000까지도 상승)으로 나오고 0.33ms정도까지 소모하는 문제가 발생

        /*[HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.CurrentZoom), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CurrentZoomPreFix(CameraDriver __instance, ref CameraZoomRange __result)
        {
            //displayDistance를 기준으로 Min을 산정함
            //해당 값을 ProgressBar, Item Count 등에 사용(Closest 아니면 무시하게 되어있음)
            double rangeMin = SimpleCameraModSetting.modSetting.displayDistance;
            //double rangeMax = __instance.config.sizeRange.max;
            //if (rangeMax > 60) rangeMax = 60;
            if ((double)__instance.ZoomRootSize < rangeMin + 1.0)
            {
                //아마 Closest 말고 다른 요소는 사용하지 않음
                __result = CameraZoomRange.Closest;
                return false;
            }
            return true;
*//*            if ((double)__instance.ZoomRootSize < (double)rangeMax * 0.23000000417232513)
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
            return false;*//*
        }*/

    }
}
