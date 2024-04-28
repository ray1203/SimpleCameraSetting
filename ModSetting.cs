using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using UnityEngine.XR;

namespace SimpleCameraSetting
{
    public class ModSetting : ModSettings
    {
        public IntRange sizeRange=new IntRange(0,60);
        public float zoomSpeed = 2f;
        public bool smoothZoom = false;
        public float silhouetteDistance = 60f;
        public override void ExposeData()
        {
            Scribe_Values.Look<IntRange>(ref sizeRange, "sizeRange", new IntRange(0, 60));
            Scribe_Values.Look<float>(ref zoomSpeed, "zoomSpeed", 2f);
            Scribe_Values.Look<bool>(ref smoothZoom, "smoothZoom", false);
            Scribe_Values.Look<float>(ref silhouetteDistance, "silhouetteDistance", 60f);
            base.ExposeData();
        }
    }
    public class SimpleCameraModSetting : Mod
    {
        public static ModSetting cameraSetting;
        public SimpleCameraModSetting(ModContentPack content) : base(content)
        {
            cameraSetting=GetSettings<ModSetting>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Zoom Range");
            listingStandard.IntRange(ref cameraSetting.sizeRange, 0, 100);
            
            //cameraSetting.zoomSpeed=listingStandard.SliderLabeled("Zoom Speed",cameraSetting.zoomSpeed, 0.1f, 10f);

            cameraSetting.zoomSpeed = Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), cameraSetting.zoomSpeed, 0.1f, 10f,
                false, string.Format("{0} {1:F1}", (object)"Zoom Speed".Translate(), (object)cameraSetting.zoomSpeed),
                (string)null, (string)null, -1f);
            listingStandard.CheckboxLabeled("Smooth Zoom", ref cameraSetting.smoothZoom);

            cameraSetting.silhouetteDistance= Widgets.HorizontalSlider(listingStandard.GetRect(28f, 1f), cameraSetting.silhouetteDistance, 30f, 100f,
                false, string.Format("{0} {1:F1}", (object)"Silhouette Distance".Translate(), (object)cameraSetting.silhouetteDistance),
                (string)null, "Set 100 to disable setting", -1f);

            //listingStandard.CheckboxLabeled("Follow Selected", ref cameraSetting.followSelected, "Camera focus on selected pawn(Uses Vanila Options)");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
            
        }
        public override void WriteSettings()
        {
            if (cameraSetting.sizeRange.min == cameraSetting.sizeRange.max)
            {
                if (cameraSetting.sizeRange.min == 0) cameraSetting.sizeRange.max++;
                else cameraSetting.sizeRange.min--;
            }
            CameraConfigPatch.ConfigPatch();
            base.WriteSettings();
        }
        public override string SettingsCategory()
        {
            return "SimpleCameraSetting".Translate();
        }
    }
}
