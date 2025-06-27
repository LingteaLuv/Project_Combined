using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStateMachineBehaviour : StateMachineBehaviour
{
    [SerializeField] private int targetLayerIndex;
    [SerializeField] private float targetWeight;
    [SerializeField] private float duration;
    [SerializeField] private WeightChangeType changeType = WeightChangeType.Smooth;

    [Header("PeakAndDrop 설정")]
    [SerializeField] private float peakWeight = 1.0f; // 최고점 가중치
    [SerializeField] private float peakPosition = 0.5f; // 최고점 위치 (0~1)

    [SerializeField] private float timer;
    [SerializeField] private float startWeight;
    [SerializeField] private bool isWeightChanging;

    public enum WeightChangeType
    {
        Smooth,     // 부드럽게 변경
        Linear,     // 선형 변경
        EaseInOut,  // 가속/감속 변경
        PeakAndDrop // 반은 상승 반은 하락
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 초기화
        timer = 0f;
        startWeight = animator.GetLayerWeight(targetLayerIndex);
        isWeightChanging = true;
        Debug.Log($"가중치 변경 시작: {startWeight} → {targetWeight}");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isWeightChanging) return;

        timer += Time.deltaTime;

        // 진행률 계산 (0~1)
        float progress = Mathf.Clamp01(timer / duration);

        // 변경 타입에 따른 가중치 계산
        float currentWeight = CalculateWeight(progress);

        // 레이어 가중치 적용
        animator.SetLayerWeight(targetLayerIndex, currentWeight);

        // 완료 체크
        if (progress >= 1.0f)
        {
            isWeightChanging = false;
            animator.SetLayerWeight(targetLayerIndex, targetWeight); // 최종값으로 정확히 설정
            Debug.Log($"가중치 변경 완료: {targetWeight}");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(targetLayerIndex, targetWeight); // 최종값으로 정확히 설정
    }

    /// <summary>
    /// 변경 타입에 따른 가중치 계산
    /// </summary>
    private float CalculateWeight(float progress)
    {
        float normalizedValue = 0f;

        switch (changeType)
        {
            case WeightChangeType.Smooth:
                // Mathf.SmoothStep 사용 (부드러운 시작과 끝)
                normalizedValue = Mathf.SmoothStep(0f, 1f, progress);
                break;

            case WeightChangeType.Linear:
                // 선형 변경
                normalizedValue = progress;
                break;

            case WeightChangeType.EaseInOut:
                // Sin 곡선을 이용한 가속/감속
                normalizedValue = Mathf.Sin(progress * Mathf.PI * 0.5f);
                break;

            case WeightChangeType.PeakAndDrop:
                // 중간에 최고점을 찍고 떨어지는 곡선
                return CalculatePeakAndDrop(progress);
        }

        // 시작 가중치에서 목표 가중치로 보간
        return Mathf.Lerp(startWeight, targetWeight, normalizedValue);
    }

    /// <summary>
    /// PeakAndDrop 전용 계산 함수
    /// </summary>
    private float CalculatePeakAndDrop(float progress)
    {
        if (progress <= peakPosition)
        {
            // 상승 구간: startWeight → peakWeight
            float upProgress = progress / peakPosition;
            // 부드러운 상승을 위해 SmoothStep 사용
            float smoothUpProgress = Mathf.SmoothStep(0f, 1f, upProgress);
            return Mathf.Lerp(startWeight, peakWeight, smoothUpProgress);
        }
        else
        {
            // 하강 구간: peakWeight → targetWeight
            float downProgress = (progress - peakPosition) / (1f - peakPosition);
            // 부드러운 하강을 위해 SmoothStep 사용
            float smoothDownProgress = Mathf.SmoothStep(0f, 1f, downProgress);
            return Mathf.Lerp(peakWeight, targetWeight, smoothDownProgress);
        }
    }
}
/*
 {[SerializeField] private int targetLayerIndex;
    [SerializeField] private float targetWeight;
    [SerializeField] private float weightChangeSpeed;
    [SerializeField] private float duration;

    [SerializeField] private WeightChangeType changeType = WeightChangeType.Smooth;

    [SerializeField] private float timer;
    [SerializeField] private float startWeight;
    [SerializeField] private bool isWeightChanging;

    public enum WeightChangeType
    {
        Smooth,     // 부드럽게 변경
        Linear,     // 선형 변경
        EaseInOut,   // 가속/감속 변경
        PeakAndDrop //반은 상승 반은 하락
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 초기화
        timer = 0f;
        startWeight = animator.GetLayerWeight(targetLayerIndex);
        isWeightChanging = true;

        Debug.Log($"가중치 변경 시작: {startWeight} → {targetWeight}");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isWeightChanging) return;

        timer += Time.deltaTime;

        // 진행률 계산 (0~1)
        float progress = Mathf.Clamp01(timer / duration);

        // 변경 타입에 따른 가중치 계산
        float currentWeight = CalculateWeight(progress);

        // 레이어 가중치 적용
        animator.SetLayerWeight(targetLayerIndex, currentWeight);

        // 완료 체크
        if (progress >= 1.0f)
        {
            isWeightChanging = false;
            animator.SetLayerWeight(targetLayerIndex, targetWeight); // 최종값으로 정확히 설정
            Debug.Log($"가중치 변경 완료: {targetWeight}");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(targetLayerIndex, targetWeight); // 최종값으로 정확히 설정
    }

    /// <summary>
    /// 변경 타입에 따른 가중치 계산
    /// </summary>
    private float CalculateWeight(float progress)
    {
        float normalizedValue = 0f;

        switch (changeType)
        {
            case WeightChangeType.Smooth:
                // Mathf.SmoothStep 사용 (부드러운 시작과 끝)
                normalizedValue = Mathf.SmoothStep(0f, 1f, progress);
                break;

            case WeightChangeType.Linear:
                // 선형 변경
                normalizedValue = progress;
                break;

            case WeightChangeType.EaseInOut:
                // Sin 곡선을 이용한 가속/감속
                normalizedValue = Mathf.Sin(progress * Mathf.PI * 0.5f);
                break;
            case WeightChangeType.PeakAndDrop: // 새로운 타입 추가
                                               // 중간에 최고점을 찍고 떨어지는 곡선
                if (progress <= 0.5f)
                {
                    // 0 -> 0.5 구간: 상승 (0 -> 1)
                    float upProgress = progress / 0.5f;
                    return Mathf.Lerp(startWeight, 1.0f, upProgress);
                }
                else
                {
                    // 0.5 -> 1 구간: 하강 (1 -> targetWeight)
                    float downProgress = (progress - 0.5f) / 0.5f;
                    return Mathf.Lerp(1.0f, targetWeight, downProgress);
                }
        }

        // 시작 가중치에서 목표 가중치로 보간
        return Mathf.Lerp(startWeight, targetWeight, normalizedValue);
    }
}
*/

// 사용 예시 - 다른 StateMachineBehaviour에서 활용하는 방법
/*
public class AttackState : StateMachineBehaviour
{
    [Header("Layer Weight Control")]
    [SerializeField] private int attackLayerIndex = 2;
    [SerializeField] private float weightChangeSpeed = 3.0f;

    private float timer;
    private float startWeight;
    private bool enterWeightChange = true;
    private bool exitWeightChange = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 레이어 가중치를 1로 서서히 변경
        timer = 0f;
        startWeight = animator.GetLayerWeight(attackLayerIndex);
        enterWeightChange = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Enter 시 가중치 증가
        if (enterWeightChange)
        {
            timer += Time.deltaTime * weightChangeSpeed;
            float currentWeight = Mathf.Lerp(startWeight, 1.0f, timer);
            animator.SetLayerWeight(attackLayerIndex, currentWeight);

            if (timer >= 1.0f)
            {
                enterWeightChange = false;
                animator.SetLayerWeight(attackLayerIndex, 1.0f);
            }
        }

        // Exit 조건 체크 (애니메이션 거의 끝날 때)
        if (stateInfo.normalizedTime >= 0.8f && !exitWeightChange)
        {
            exitWeightChange = true;
            timer = 0f;
            startWeight = animator.GetLayerWeight(attackLayerIndex);
        }

        // Exit 시 가중치 감소
        if (exitWeightChange)
        {
            timer += Time.deltaTime * weightChangeSpeed;
            float currentWeight = Mathf.Lerp(startWeight, 0.0f, timer);
            animator.SetLayerWeight(attackLayerIndex, currentWeight);

            if (timer >= 1.0f)
            {
                animator.SetLayerWeight(attackLayerIndex, 0.0f);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 확실히 0으로 설정
        animator.SetLayerWeight(attackLayerIndex, 0.0f);
    }*/

