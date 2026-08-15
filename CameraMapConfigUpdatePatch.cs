
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

        [HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.Update))]
        [HarmonyPostfix]
        public static void FollowPostfix(CameraDriver __instance)
        {
            //로딩중엔 바닐라 Update도 일찍 return하니 여기도 막음
            if (LongEventHandler.ShouldWaitForEvent || Find.CurrentMap == null)
                return;
            CameraMapConfig config = __instance.config;
            if (config == null || !config.followSelected)
                return;

            List<Pawn> selectedPawns = Find.Selector.SelectedPawns;

            //선택 폰 없을 때
            if (selectedPawns.Empty())
            {
                if (SimpleCameraModSetting.modSetting.autoOffFollow)
                {
                    config.followSelected = false;
                    if (SimpleCameraModSetting.modSetting.followMessage)
                        Messages.Message("Camera Following Off", new MessageTypeDef(), false);
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
            if (num > 0)
            {
                Vector3 target = zero / (float)num;
                //JumpToCurrentMapLoc으로 rootPos(카메라 위치) 세팅
                __instance.JumpToCurrentMapLoc(target);
                //Update의 위치적용은 이미 지나가서 같은 프레임 반영되게 다시 호출
                Refs.applyPositionToGameObject(__instance);
            }
        }
    }
}
