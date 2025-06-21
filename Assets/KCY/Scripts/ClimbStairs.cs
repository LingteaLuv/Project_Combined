using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbStairs : MonoBehaviour
{
    private Rigidbody _rb;
    private CapsuleCollider _capCol;
    //private bool _isClimbing = false;
    //public float ClimbTimer = 0;   레이를 검사하는 과정을 늘릴 예정 입니다.
    //public float CTCali = 0.1f;


    [Header("계단 감지 설정")]

    // 계단으로 인지할 수 있는 최대 높이를 설정
    [SerializeField] private float _maxStepHeight = 3f;
    // 플레이어로 부터 계단을 감지할 거리를 설정
    [SerializeField] private float _scanDis = 0.2f;
    // 이름만 다를뿐 ground레이어를 입력한다.
    [SerializeField] private LayerMask _stairMask; 


    // 벽에 끼이는 것을 방지해야 하는데...... - 팀원 분들께 상의 드리기
    // 확인해 보니 캡슐형 레이케스트를 쏠때 물체에 맞는 점은 제일 처음맞는 면입니다.
    // 그런데 만약 계단이 평면이라면 캡슐과 계단의 수직면이 맞닿는 것은 선이됩니다. 즉 이 선에서 랜덤으로 점이 결정됩니다.
    // 만약 해당 부분에서 가장 먼저 닿는 점이 0에 가까우면 계단을 넘지 못하는 (1프레임) 그래서 버벅이게 될 것 같습니다.
    // 이를 방지하고 싶은데 생각이 나지않습니다. 혹시 죄송하지만 이를 어떻게 해결하면 좋은지 여쭤보고 싶어서 말씀을 드렸습니다.

    public void TryClimbStairs()
    {

        float radius = _capCol.radius;
   

        //플레이어의 발위치(정확하게는 캐릭터 콜라이더의 끝 위치를 설정)
        // = 플레어 위치 중심(우리 플레이어 프리팹 역시 콜라이더의 중앙) - 콜라이더의 절반 높이를 뺌 +
        // 애니메이션에 의한 transform과 콜라이더 불일치 방지 (숙이는 경우 콜라이더와 트랜스폼의 중심점이 다를 수 있음)
        float playerFootY = transform.position.y - (_capCol.height / 2f) + _capCol.center.y;

        Vector3 rayStartPos =
               transform.position
               + transform.forward * 0.1f
               + Vector3.up * (_maxStepHeight + 0.02f);    
          

        // 계단 위치를 확인해서 플레이어를 계단 위로 띄우기 위한 거리 측정
        if (Physics.Raycast(rayStartPos, Vector3.down, out RaycastHit stair, _maxStepHeight, _stairMask))
        {
            float stairsY = stair.point.y;
            float moveUp = stair.point.y - playerFootY + 0.02f;

            if (moveUp > 0 && moveUp <= _maxStepHeight)
            {
                _rb.position += new Vector3(0f, moveUp, 0f);
            }
        }

        // 캡슐은 앞방향으로 쏜다.
        Vector3 capDir = transform.forward;

        // 캡슐레이 크기 설정(p1은 캡슐의 맨위, p2는 캡슐의 맨밑/ offset값을 추가하여 오류방지)
        Vector3 p1 = transform.position + Vector3.up * (_maxStepHeight + 0.05f);
        Vector3 p2 = transform.position + Vector3.up * 0.05f;
        bool ditectStair = Physics.CapsuleCast(p1, p2, radius, capDir, out RaycastHit hit, _scanDis, _stairMask);
        if (!ditectStair) { return; }

        // 범위가 설정된 캡슐을 앞방향으로 쏴서 ground레이어에 반응하도록 설계
        // 다양한 계단을 커버하기 위해 레이에 맞는 오브젝트의 y값과 발바닥의 높이 만큼 올려주는 모션을 취할 예정 - 직선이 대체
        // - 그러나 해당의 경우 걸릴 수 있음
       

    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capCol = GetComponent<CapsuleCollider>();
    }
    /*private void FixedUpdate()
    {
        if (_isClimbing)
        {
            ClimbTimer -= Time.fixedDeltaTime;
            if (ClimbTimer <= 0f)
            {
                _isClimbing = false;
            }

        }
    }
    */
}
