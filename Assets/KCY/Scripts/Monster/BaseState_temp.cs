using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState_temp
{
    // 상태 패턴으로 제작하기 위한 임시용 파일 입니다. 

    public bool HasPhysics;


    // 상태가 시작 될때
    public abstract void Enter();

    // 해당 상태에서 동작을 담당
    public abstract void Update();

    // 사용하지 않는 상태들도 있기 때문에 가상함수로 선언 (오브젝트의 물리는 여기서 담당함)
    public virtual void FixedUpdate() { }

    // 상태가 끝날 때
    public abstract void Exit();
}
