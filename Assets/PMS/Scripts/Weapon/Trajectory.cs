using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    // 궤적을 그리기 위한 LineRenderer
    private LineRenderer _renderer;
    private Camera _camera;

    public void UpdateTrajectory(GameObject bullet, float bulletSpeed)
    {
        // 궤적을 그리는데 사용될 정점 개수
        int pointCount = 30;
        // 정점 간의 시간차
        float deltaTime = 0.1f;

        Vector3[] trajectorys = new Vector3[pointCount];
        // 궤적의 시작점
        Vector3 startPos = transform.position;

        // 무기 방향 → 카메라 방향
        transform.rotation = Quaternion.Euler
        (_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, 0);

        // 발사체 방향 = 무기 방향
        bullet.transform.rotation = transform.rotation;

        float speed = bulletSpeed;

        // 궤적 및 발사체 시작 속도
        Vector3 startVel = transform.forward * speed;

        // 궤적 발사체 운동 동기화, 계산
        for (int i = 0; i < pointCount; i++)
        {
            trajectorys[i] = CalculatePoint(startPos, startVel, deltaTime * i);
        }

        // 
        _renderer.positionCount = pointCount;

        // 렌더러로 계산된 궤적 표시
        _renderer.SetPositions(trajectorys);
    }

    private Vector3 CalculatePoint(Vector3 startPos, Vector3 startVel, float time)
    {
        return startPos + startVel * time;
    }

    private void Init()
    {
        _renderer = GetComponent<LineRenderer>();
        _camera = transform.parent.GetComponentInChildren<Camera>();
    }
}
