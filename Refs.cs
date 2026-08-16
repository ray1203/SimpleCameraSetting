using HarmonyLib;
using RimWorld;
using System;
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
        //ApplyPositionToGameObject가 private라 리플렉션으로 뺌
        //매 프레임 따라가기에서 카메라 위치 바로 반영하려고 씀
        public static readonly Action<CameraDriver> applyPositionToGameObject;
        //rootPos도 private. 스무딩 시작할 때 현재 카메라 위치 시드용으로 읽음
        public static readonly AccessTools.FieldRef<CameraDriver, Vector3> rootPos;
        static Refs()
        {
            Refs.desiredSize = AccessTools.FieldRefAccess<CameraDriver, float>(nameof(desiredSize));
            Refs.f_desiredSize = AccessTools.Field(typeof(CameraDriver), nameof(desiredSize));
            Refs.applyPositionToGameObject = AccessTools.MethodDelegate<Action<CameraDriver>>(
                AccessTools.Method(typeof(CameraDriver), "ApplyPositionToGameObject"));
            Refs.rootPos = AccessTools.FieldRefAccess<CameraDriver, Vector3>(nameof(rootPos));
        }
    }
}
