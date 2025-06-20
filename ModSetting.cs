﻿using Verse;
using UnityEngine;
using System;
using BetterKeybinding;

namespace SimpleCameraSetting
{
    public class ModSetting : ModSettings
    {
        public IntRange sizeRange;
        public float zoomSpeed;
        public bool smoothZoom;
        public float silhouetteDistance;
        public float displayDistance;
        private static KeyBind _followCameraKey;
        public bool hideBracket;
        public bool zoomToMouse;

        public bool zoomDebugMessage;

        public static KeyBind followCameraKey
        {
            // Method get_FollowMeKey with token 06000042
            get
            {
                if (ModSetting._followCameraKey == null)
                    ModSetting._followCameraKey = new KeyBind("", KeyCode.Backspace, EventModifiers.None);
                return ModSetting._followCameraKey;
            }
        }
        public bool autoOffFollow;

        public float moveSpeedScale_1;
        public float moveSpeedScale_3;
        public float moveSpeedScale_5;
        public float moveSpeedScale_10;
        public float moveSpeedScale_20;
        public float moveSpeedScale_40;
        public float moveSpeedScale_60;
        public float moveSpeedScale_100;
        public float moveSpeedScale_200;
        public ModSetting()
        {
            SetDefault();
        }
        public void SetDefault()
        {
            sizeRange = new IntRange(0, 100);
            zoomSpeed = 2f;
            smoothZoom = false;
            silhouetteDistance = 60f;
            displayDistance = 11f;
            ModSetting._followCameraKey = new KeyBind("", KeyCode.Backspace, EventModifiers.None);

            hideBracket = true;
            autoOffFollow = true;
            zoomToMouse = true;
            zoomDebugMessage = false;

            moveSpeedScale_1 = 0.2f;
            moveSpeedScale_3 = 0.4f;
            moveSpeedScale_5 = 0.8f;
            moveSpeedScale_10 = 1.5f;
            moveSpeedScale_20 = 2f;
            moveSpeedScale_40 = 2f;
            moveSpeedScale_60 = 2f;
            moveSpeedScale_100 = 2f;
            moveSpeedScale_200 = 2f;
        }
        public override void ExposeData()
        {
            Scribe_Values.Look<IntRange>(ref sizeRange, "sizeRange", new IntRange(0, 100));
            Scribe_Values.Look<float>(ref zoomSpeed, "zoomSpeed", 2f);
            Scribe_Values.Look<bool>(ref smoothZoom, "smoothZoom", false);
            Scribe_Values.Look<float>(ref silhouetteDistance, "silhouetteDistance", 60f);
            Scribe_Values.Look<float>(ref displayDistance, "displayDistance", 11f);
            Scribe_Deep.Look<KeyBind>(ref ModSetting._followCameraKey, "followCameraKey", Array.Empty<object>());
            Scribe_Values.Look<bool>(ref hideBracket, "hideBracket", true);
            Scribe_Values.Look<bool>(ref autoOffFollow, "autoOffFollow", true);
            Scribe_Values.Look<bool>(ref zoomToMouse, "zoomToMouse", Prefs.ZoomToMouse);
            Scribe_Values.Look<bool>(ref zoomDebugMessage, "zoomDebugMessage", false);


            Scribe_Values.Look(ref moveSpeedScale_1, "moveSpeedScale_1", 0.2f);
            Scribe_Values.Look(ref moveSpeedScale_3, "moveSpeedScale_3", 0.4f);
            Scribe_Values.Look(ref moveSpeedScale_5, "moveSpeedScale_5", 0.8f);
            Scribe_Values.Look(ref moveSpeedScale_10, "moveSpeedScale_10", 1.5f);
            Scribe_Values.Look(ref moveSpeedScale_20, "moveSpeedScale_20", 2f);
            Scribe_Values.Look(ref moveSpeedScale_40, "moveSpeedScale_40", 2f);
            Scribe_Values.Look(ref moveSpeedScale_60, "moveSpeedScale_60", 2f);
            Scribe_Values.Look(ref moveSpeedScale_100, "moveSpeedScale_100", 2f);
            Scribe_Values.Look(ref moveSpeedScale_200, "moveSpeedScale_200", 2f);
            base.ExposeData();

        }
    }
    public class SimpleCameraModSetting : Mod
    {
        public static ModSetting modSetting;
        public SimpleCameraModSetting(ModContentPack content) : base(content)
        {
            modSetting = GetSettings<ModSetting>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            float columnWidth = (inRect.width - 34f) / 2f;
            listingStandard.ColumnWidth = columnWidth;

            listingStandard.Begin(inRect);

            // Zoom range
            listingStandard.Label("ZoomRange".Translate());
            listingStandard.Gap(-4);
            listingStandard.IntRange(ref modSetting.sizeRange, 0, 200);

            // Zoom speed
            Rect zoomSpeedRect = listingStandard.GetRect(Text.LineHeight);
            Widgets.Label(zoomSpeedRect, "ZoomSpeed".Translate() + ": " + modSetting.zoomSpeed.ToString("F1"));
            modSetting.zoomSpeed = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), modSetting.zoomSpeed, 0.1f, 10f);

            listingStandard.GapLine();
            listingStandard.Gap();

            // Silhouette Distance (with tooltip)
            Rect silRect = listingStandard.GetRect(Text.LineHeight);
            Widgets.Label(silRect, "SilhouetteDistance".Translate() + ": " + modSetting.silhouetteDistance.ToString("F1"));
            TooltipHandler.TipRegion(silRect, "SilhouetteDistanceTooltip".Translate());
            modSetting.silhouetteDistance = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), modSetting.silhouetteDistance, 30f, 100f);

            // Display Distance (with tooltip)
            Rect dispRect = listingStandard.GetRect(Text.LineHeight);
            Widgets.Label(dispRect, "DisplayDistance".Translate() + ": " + modSetting.displayDistance.ToString("F1"));
            TooltipHandler.TipRegion(dispRect, "DisplayDistanceTooltip".Translate());
            modSetting.displayDistance = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), modSetting.displayDistance, 1f, 60f);

            listingStandard.CheckboxLabeled("SmoothZoom".Translate(), ref modSetting.smoothZoom, "");
            listingStandard.CheckboxLabeled("ZoomToMouse".Translate(), ref modSetting.zoomToMouse);

            listingStandard.GapLine();
            listingStandard.Gap();

            ModSetting.followCameraKey.label = "followCameraKey".Translate();
            ModSetting.followCameraKey.Draw(listingStandard.GetRect(30f, 1f), 6);

            listingStandard.CheckboxLabeled("HideBracket".Translate(), ref modSetting.hideBracket, "HideBracketTooltip".Translate());
            listingStandard.CheckboxLabeled("AutoOffFollow".Translate(), ref modSetting.autoOffFollow, "AutoOffFollowTooltip".Translate());

            listingStandard.GapLine();
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("ZoomDebugMessage".Translate(), ref modSetting.zoomDebugMessage, "ZoomDebugMessageTooltip".Translate());

            if (listingStandard.ButtonText("DefaultValue".Translate()))
                modSetting.SetDefault();

            listingStandard.NewColumn();

            DrawCameraSpeedSetting(listingStandard, "CameraSpeed1".Translate(), ref modSetting.moveSpeedScale_1);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed3".Translate(), ref modSetting.moveSpeedScale_3);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed5".Translate(), ref modSetting.moveSpeedScale_5);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed10".Translate(), ref modSetting.moveSpeedScale_10);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed20".Translate(), ref modSetting.moveSpeedScale_20);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed40".Translate(), ref modSetting.moveSpeedScale_40);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed60".Translate(), ref modSetting.moveSpeedScale_60);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed100".Translate(), ref modSetting.moveSpeedScale_100);
            DrawCameraSpeedSetting(listingStandard, "CameraSpeed200".Translate(), ref modSetting.moveSpeedScale_200);

            listingStandard.End();

            base.DoSettingsWindowContents(inRect);
        }
        public override void WriteSettings()
        {
            if (modSetting.sizeRange.min == modSetting.sizeRange.max)
            {
                if (modSetting.sizeRange.min == 0) modSetting.sizeRange.max++;
                else modSetting.sizeRange.min--;
            }
            Prefs.ZoomToMouse = modSetting.zoomToMouse;
            CameraConfigPatch.ConfigPatch();
            base.WriteSettings();
        }
        public override string SettingsCategory()
        {
            return "SimpleCameraSetting".Translate();
        }


        public void DrawCameraSpeedSetting(Listing_Standard listingStandard, string label, ref float currentValue)
        {
            Rect rect = listingStandard.GetRect(Text.LineHeight);
            Rect labelRect = new Rect(rect.x, rect.y, 200f, rect.height);
            Rect valueRect = new Rect(labelRect.xMax, rect.y, 30f, rect.height);
            Rect buttonRect = new Rect(valueRect.xMax + 5f, rect.y, 30f, rect.height);

            Widgets.Label(labelRect, label);
            Widgets.Label(valueRect, currentValue.ToString("0.0"));

            if (Widgets.ButtonText(buttonRect, "<<") && currentValue > 0f)
            {
                currentValue -= 1f;
            }
            buttonRect.x += buttonRect.width + 5f;
            if (Widgets.ButtonText(buttonRect, "<") && currentValue > 0.1f)
            {
                currentValue -= 0.1f;
            }
            buttonRect.x += buttonRect.width + 5f;
            if (Widgets.ButtonText(buttonRect, ">") && currentValue < 9.9f)
            {
                currentValue += 0.1f;
            }
            buttonRect.x += buttonRect.width + 5f;
            if (Widgets.ButtonText(buttonRect, ">>") && currentValue < 10f)
            {
                currentValue += 1f;
            }
            if (currentValue < 0f) currentValue = 0f;
            if (currentValue > 10f) currentValue = 10f;
            //listingStandard.GapLine();
            listingStandard.Gap(2f);
        }

    }
}
