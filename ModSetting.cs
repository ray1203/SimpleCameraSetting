using Verse;
using UnityEngine;

namespace SimpleCameraSetting
{
    public class ModSetting : ModSettings
    {
        public IntRange sizeRange;
        public float zoomSpeed;
        public bool smoothZoom;
        public float silhouetteDistance;
        public float displayDistance;
        public float moveSpeedScale_1;
        public float moveSpeedScale_3;
        public float moveSpeedScale_5;
        public float moveSpeedScale_10;
        public float moveSpeedScale_20;
        public float moveSpeedScale_40;
        public float moveSpeedScale_60;
        public float moveSpeedScale_100;
        public ModSetting()
        {
            SetDefault();
        }
        public void SetDefault()
        {
            sizeRange = new IntRange(0, 60);
            zoomSpeed = 2f;
            smoothZoom = false;
            silhouetteDistance = 60f;
            displayDistance = 11f;
            moveSpeedScale_1 = 0.2f;
            moveSpeedScale_3 = 0.4f;
            moveSpeedScale_5 = 0.8f;
            moveSpeedScale_10 = 1.5f;
            moveSpeedScale_20 = 2f;
            moveSpeedScale_40 = 3f;
            moveSpeedScale_60 = 4f;
            moveSpeedScale_100 = 5f;
        }
        public override void ExposeData()
        {
            Scribe_Values.Look<IntRange>(ref sizeRange, "sizeRange", new IntRange(0, 60));
            Scribe_Values.Look<float>(ref zoomSpeed, "zoomSpeed", 2f);
            Scribe_Values.Look<bool>(ref smoothZoom, "smoothZoom", false);
            Scribe_Values.Look<float>(ref silhouetteDistance, "silhouetteDistance", 60f);
            Scribe_Values.Look<float>(ref displayDistance, "displayDistance", 11f);

            Scribe_Values.Look(ref moveSpeedScale_1, "moveSpeedScale_1", 0.2f);
            Scribe_Values.Look(ref moveSpeedScale_3, "moveSpeedScale_3", 0.4f);
            Scribe_Values.Look(ref moveSpeedScale_5, "moveSpeedScale_5", 0.8f);
            Scribe_Values.Look(ref moveSpeedScale_10, "moveSpeedScale_10", 1.5f);
            Scribe_Values.Look(ref moveSpeedScale_20, "moveSpeedScale_20", 2f);
            Scribe_Values.Look(ref moveSpeedScale_40, "moveSpeedScale_40", 3f);
            Scribe_Values.Look(ref moveSpeedScale_60, "moveSpeedScale_60", 4f);
            Scribe_Values.Look(ref moveSpeedScale_100, "moveSpeedScale_100", 5f);
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
            Rect leftRect = new Rect(inRect.x, inRect.y, columnWidth, inRect.height);
            Rect rightRect = new Rect(inRect.x + columnWidth + 34f, inRect.y, columnWidth, inRect.height);

            listingStandard.Begin(inRect);
            listingStandard.Label("Zoom Range");
            listingStandard.IntRange(ref modSetting.sizeRange, 0, 100);
            listingStandard.GapLine();

            //cameraSetting.zoomSpeed=listingStandard.SliderLabeled("Zoom Speed",cameraSetting.zoomSpeed, 0.1f, 10f);

            modSetting.zoomSpeed = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), modSetting.zoomSpeed, 0.1f, 10f,
                false, string.Format("{0} {1:F1}", (object)"Zoom Speed".Translate(), (object)modSetting.zoomSpeed),
                (string)null, (string)null, -1f);
            listingStandard.GapLine();
            listingStandard.Gap();

            modSetting.silhouetteDistance = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), modSetting.silhouetteDistance, 30f, 100f,
                false, "",string.Format("{0} {1:F1}", (object)"Silhouette Distance".Translate(), (object)modSetting.silhouetteDistance)
                , "set 100 to disable", -1f);
            listingStandard.GapLine();
            listingStandard.Gap();

            modSetting.displayDistance = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), modSetting.displayDistance, 1f, 60f,
                false, "", string.Format("{0} {1:F1}", (object)"Display Distance".Translate(), (object)modSetting.displayDistance)
                , "show item count, progress bar", -1f);
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.CheckboxLabeled("Smooth Zoom", ref modSetting.smoothZoom);

            //listingStandard.CheckboxLabeled("Follow Selected", ref cameraSetting.followSelected, "Camera focus on selected pawn(Uses Vanila Options)");

            listingStandard.GapLine();
            listingStandard.Gap();

            if (listingStandard.ButtonText("Default Value")) modSetting.SetDefault();

            listingStandard.NewColumn();


            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<1)", ref modSetting.moveSpeedScale_1);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<3)", ref modSetting.moveSpeedScale_3);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<5)", ref modSetting.moveSpeedScale_5);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<10)", ref modSetting.moveSpeedScale_10);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<20)", ref modSetting.moveSpeedScale_20);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<40)", ref modSetting.moveSpeedScale_40);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<60)", ref modSetting.moveSpeedScale_60);
            DrawCameraSpeedSetting(listingStandard, "Camera Speed (zoom<100)", ref modSetting.moveSpeedScale_100);


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

            Widgets.Label(labelRect, label + " : ");
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
            listingStandard.GapLine();
        }

    }
}
