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

        //리플렉션 실패 시 폴백용 더미
        static float _dummySize;
        static Vector3 _dummyRootPos;
        static ref float DummySizeRef(CameraDriver _) => ref _dummySize;
        static ref Vector3 DummyRootPosRef(CameraDriver _) => ref _dummyRootPos;

        static Refs()
        {
            f_desiredSize = AccessTools.Field(typeof(CameraDriver), nameof(desiredSize));

            try { desiredSize = AccessTools.FieldRefAccess<CameraDriver, float>(nameof(desiredSize)); }
            catch (Exception e)
            {
                Log.Error("[SimpleCameraSetting] CameraDriver.desiredSize 참조 실패 - 줌속도 설정이 동작 안 할 수 있음: " + e.Message);
                desiredSize = DummySizeRef;
            }

            try
            {
                var m = AccessTools.Method(typeof(CameraDriver), "ApplyPositionToGameObject");
                if (m == null) throw new MissingMethodException("CameraDriver.ApplyPositionToGameObject");
                applyPositionToGameObject = (Action<CameraDriver>)m.CreateDelegate(typeof(Action<CameraDriver>));
            }
            catch (Exception e)
            {
                Log.Error("[SimpleCameraSetting] CameraDriver.ApplyPositionToGameObject 참조 실패 - 따라가기가 1프레임 지연될 수 있음: " + e.Message);
                applyPositionToGameObject = _ => { };
            }

            try { rootPos = AccessTools.FieldRefAccess<CameraDriver, Vector3>(nameof(rootPos)); }
            catch (Exception e)
            {
                Log.Error("[SimpleCameraSetting] CameraDriver.rootPos 참조 실패 - 따라가기 스무딩 시드가 저하될 수 있음: " + e.Message);
                rootPos = DummyRootPosRef;
            }
        }
    }
}
