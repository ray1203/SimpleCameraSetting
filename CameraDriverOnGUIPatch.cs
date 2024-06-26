﻿using HarmonyLib;
using Verse;

namespace SimpleCameraSetting
{
    [HarmonyPatch]
    public static class CameraDriverOnGUIPatch
    {
        private static float desiredSize;
        private static float desiredSizeBefore = 0f;
        private static Message zoomMessage = new Message("", new MessageTypeDef());
        //public static bool followCameraFlag = false;
        [HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.CameraDriverOnGUI))]
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
                else if (desiredSize < 100f) __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_100;
                else __instance.config.moveSpeedScale = SimpleCameraModSetting.modSetting.moveSpeedScale_200;

                //현재 줌을 메시지로 출력
                if (SimpleCameraModSetting.modSetting.zoomDebugMessage)
                {
                    zoomMessage.ResetTimer();
                    zoomMessage.text = string.Format("Current Zoom : {0:F2}          ", desiredSize);
                    Messages.Message(zoomMessage);
                }
                desiredSizeBefore = desiredSize;
            }
            if (ModSetting.followCameraKey.JustPressed)
            {
                //followCameraFlag = !followCameraFlag;
                Current.CameraDriver.config.followSelected = !Current.CameraDriver.config.followSelected;
                Messages.Message("Camera Following " + (Current.CameraDriver.config.followSelected ? "On" : "Off"), new MessageTypeDef());

            }
            return true;
        }
    }
}
