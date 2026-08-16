
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace SimpleCameraSetting
{
    //CameraFollowing 기능 관련
    //메시지 띄우기, 다른 림 Follow 시 부드럽게 이동 등
    //ConfigFixedUpdate_60(60Hz)에선 배속일 때 폰이 떨려서 매 프레임 Update Postfix로 처리
    //일단 클래스명과 파일명은 유지
    [HarmonyPatch]
    public static class CameraMapConfigUpdatePatch
    {
        static List<Pawn> lastSelectedPawns = new List<Pawn>();

        //스무딩(followSmoothTime > 0)용 상태
        static Vector3 smoothedPos;
        static Vector3 smoothVel;
        static bool wasFollowing;
        //목표가 이만큼(셀) 넘게 튀면 스무딩 대신 스냅 (텔레포트/맵이동 등)
        const float SnapSq = 15f * 15f;

        [HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.Update))]
        [HarmonyPostfix]
        public static void FollowPostfix(CameraDriver __instance)
        {
            //로딩중엔 바닐라 Update도 일찍 return하니 여기도 막음
            if (LongEventHandler.ShouldWaitForEvent || Find.CurrentMap == null)
            { wasFollowing = false; return; }
            CameraMapConfig config = __instance.config;
            if (config == null || !config.followSelected)
            { wasFollowing = false; return; }

            List<Pawn> selectedPawns = Find.Selector.SelectedPawns;

            //선택 폰 없을 때
            if (selectedPawns.Empty())
            {
                if (SimpleCameraModSetting.modSetting.autoOffFollow)
                {
                    config.followSelected = false;
                    if (SimpleCameraModSetting.modSetting.followMessage)
                        Messages.Message("Camera Following Off", new MessageTypeDef(), false);
                    wasFollowing = false;
                    return;
                }
                selectedPawns = lastSelectedPawns;
            }
            else { lastSelectedPawns.Clear(); lastSelectedPawns.AddRange(selectedPawns); }

            Vector3 zero = Vector3.zero;
            int num = 0;
            foreach (Pawn t in selectedPawns)
            {
                if (t.MapHeld == Find.CurrentMap)
                {
                    //이번 프레임 tween 갱신하고 읽어야 폰 그려지는 위치랑 카메라가 맞음
                    //TweenedPos만 씀. DrawPos엔 피격 jitter/조준 lean 섞여서 카메라 흔들림
                    t.Drawer.tweener.PreDrawPosCalculation();
                    zero += t.Drawer.tweener.TweenedPos;
                    ++num;
                }
            }
            if (num <= 0) { wasFollowing = false; return; }

            Vector3 target = zero / (float)num;
            float smoothTime = SimpleCameraModSetting.modSetting.followSmoothTime;
            Vector3 outPos;
            if (smoothTime <= 0f)
            {
                //기본 동작
                outPos = target;
                wasFollowing = false;
            }
            else
            {
                if (!wasFollowing)
                {
                    //따라가기 시작, 부드럽게 이동
                    Vector3 cur = Refs.rootPos(__instance);
                    smoothedPos = new Vector3(cur.x, target.y, cur.z);
                    smoothVel = Vector3.zero;
                }
                else if ((target - smoothedPos).sqrMagnitude > SnapSq)
                {
                    //텔레포트/맵이동 등 거리 멀어지면 맵 가로지르지 말고 스냅
                    smoothedPos = target;
                    smoothVel = Vector3.zero;
                }
                else
                {
                    smoothedPos = Vector3.SmoothDamp(smoothedPos, target, ref smoothVel, smoothTime, Mathf.Infinity, Time.deltaTime);
                }
                outPos = smoothedPos;
                wasFollowing = true;
            }

            __instance.JumpToCurrentMapLoc(outPos);
            //Update의 위치적용은 이미 지나가서 같은 프레임 반영되게 다시 호출
            Refs.applyPositionToGameObject(__instance);
        }
    }
}
