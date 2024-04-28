using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace SimpleCameraSetting
{
    [StaticConstructorOnStartup]
    internal static class Refs
    {
        public static readonly FieldInfo f_desiredSize;
        public static readonly AccessTools.FieldRef<CameraDriver, float> desiredSize;
        static Refs()
        {
            Refs.desiredSize = AccessTools.FieldRefAccess<CameraDriver, float>(nameof(desiredSize));
            Refs.f_desiredSize = AccessTools.Field(typeof(CameraDriver), nameof(desiredSize));
        }
    }
}
