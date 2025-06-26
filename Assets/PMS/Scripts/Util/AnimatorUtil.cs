using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 관련 유틸리티 함수들을 제공하는 정적 클래스
/// </summary>
public static class AnimatorUtil 
{

    /// <summary>
    /// 특정 레이어의 가중치를 즉시 설정
    /// </summary>
    /// <param name="animator">대상 애니메이터</param>
    /// <param name="layerIndex">특정 레이어 인덱스 (0이 Base Layer)</param> 
    /// <param name="weight">가중치 (0~1)</param>
    /// <param name="showLog">로그 출력 여부</param>
    /// <returns>성공 여부</returns>
    public static bool SetLayerWeight(Animator animator, int layerIndex, float weight, bool showLog = false)
    {
        if (!ValidateAnimator(animator, layerIndex))
            return false;

        weight = Mathf.Clamp01(weight);
        animator.SetLayerWeight(layerIndex, weight);

        if (showLog)
            Debug.Log($"[AnimatorUtility] 레이어 {layerIndex}의 가중치를 {weight:F2}로 설정");

        return true;
    }

    /// <summary>
    /// 레이어 이름으로 가중치 설정
    /// </summary>
    /// <param name="animator">대상 애니메이터</param>
    /// <param name="layerName">레이어 이름</param>
    /// <param name="weight">가중치 (0~1)</param>
    /// <param name="showLog">로그 출력 여부</param>
    /// <returns>성공 여부</returns>
    public static bool SetLayerWeight(Animator animator, string layerName, float weight, bool showLog = false)
    {
        int layerIndex = GetLayerIndex(animator, layerName);
        if (layerIndex == -1)
        {
            if (showLog)
                Debug.LogError($"[AnimatorUtility] 레이어 '{layerName}'을 찾을 수 없습니다.");
            return false;
        }

        return SetLayerWeight(animator, layerIndex, weight, showLog);
    }

    /// <summary>
    /// 레이어 이름으로 인덱스 찾기
    /// </summary>
    /// <param name="animator">대상 애니메이터</param>
    /// <param name="layerName">레이어 이름</param>
    /// <returns>레이어 인덱스 (-1이면 못찾음)</returns>
    public static int GetLayerIndex(Animator animator, string layerName)
    {
        if (animator == null || string.IsNullOrEmpty(layerName))
            return -1;

        for (int i = 0; i < animator.layerCount; i++)
        {
            if (animator.GetLayerName(i) == layerName)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// 애니메이터와 레이어 유효성 검사
    /// </summary>
    /// <param name="animator">대상 애니메이터</param>
    /// <param name="layerIndex">레이어 인덱스</param>
    /// <returns>유효성 여부</returns>
    private static bool ValidateAnimator(Animator animator, int layerIndex)
    {
        if (animator == null)
        {
            Debug.LogError("[AnimatorUtility] Animator가 null입니다.");
            return false;
        }

        if (layerIndex < 0 || layerIndex >= animator.layerCount)
        {
            Debug.LogError($"[AnimatorUtility] 잘못된 레이어 인덱스: {layerIndex}. 총 레이어 수: {animator.layerCount}");
            return false;
        }

        if (layerIndex == 0)
        {
            Debug.LogWarning("[AnimatorUtility] Base Layer(0)의 가중치는 변경할 수 없습니다.");
            return false;
        }

        return true;
    }
}
