using UnityEngine;

/// <summary>
/// 입력을 감지하고 외부로 전달하는 클래스입니다.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool IsOnLadder { get; private set;} 
    public bool InteractPressed { get; private set; }
    
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        MoveInput = new Vector3(x, 0, z).normalized;

        JumpPressed = Input.GetButtonDown("Jump");
        CrouchHeld = Input.GetKey(KeyCode.LeftControl);
        InteractPressed = Input.GetKeyDown(KeyCode.F);


        if (Physics.Raycast(transform.position+Vector3.up * 0.1f, transform.forward, out RaycastHit hit, 0.3f))
        {
            if (hit.collider.CompareTag("Ladder"))
            {
                IsOnLadder = true;
            }
            else
            {
                IsOnLadder = false;
            }
        }
        else
        {
            IsOnLadder = false;
        }
    }
}