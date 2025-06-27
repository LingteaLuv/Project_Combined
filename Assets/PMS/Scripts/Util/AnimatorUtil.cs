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

    //부드럽게 바꾸는데 Index를 받음
    public static Coroutine SetLayerWeightSmooth(MonoBehaviour owner, Animator animator, int layerIndex, float targetWeight, float duration = 0.3f, bool showLog = false)
    {
        if (!ValidateAnimator(animator, layerIndex))
            return null;

        return owner.StartCoroutine(LerpLayerWeight(animator, layerIndex, targetWeight, duration, showLog));
    }

    //이친구는 index대신 string으로 받음
    public static Coroutine SetLayerWeightSmooth(MonoBehaviour owner, Animator animator, string layerName, float targetWeight, float duration = 0.3f, bool showLog = false)
    {
        int layerIndex = GetLayerIndex(animator, layerName);

        if (!ValidateAnimator(animator, layerIndex))
            return null;

        return owner.StartCoroutine(LerpLayerWeight(animator, layerIndex, targetWeight, duration, showLog));
    }

    private static IEnumerator LerpLayerWeight(Animator animator, int layerIndex, float targetWeight, float duration, bool showLog)
    {
        float startWeight = animator.GetLayerWeight(layerIndex);
        float elapsed = 0f;

        targetWeight = Mathf.Clamp01(targetWeight);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, t);
            animator.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }

        animator.SetLayerWeight(layerIndex, targetWeight); // 보정

        if (showLog)
            Debug.Log($"[AnimatorUtility] 레이어 {layerIndex}의 가중치를 {targetWeight:F2}로 부드럽게 설정 완료");
    }

    //특정 레이어들을 제외한 모든 레이어 가중치를 0으로 만들때가 필요하다.
    //배열로 받아서 처리
    /*public static Coroutine SetLayerWeightSmooth(MonoBehaviour owner, Animator animator, string[] layerNames, float targetWeight, float duration = 0.3f, bool showLog = false)
    {
        int[] layerIndexs = new int[layerNames.Length];

        foreach (int index in layerIndexs)
        {
            index
        }
        GetLayerIndex(animator, layerName);

        if (!ValidateAnimator(animator, layerIndex))
            return null;

        return owner.StartCoroutine(LerpLayerWeight(animator, layerIndex, targetWeight, duration, showLog));
    }*/
}
