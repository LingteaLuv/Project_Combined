using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _vc;
    [SerializeField] private PlayerNPCInteractor _pi;
    private void Awake()
    {
        _vc = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        _pi.OnInteract += CalculatePos;
        DialogueManager.Instance.OffDialogue += disableCamera;
    }

    private void disableCamera()
    {
        _vc.Priority = 0;
    }

    
    private void CalculatePos(Transform player, Transform npc)
    {
        Vector3 between = player.position - npc.position;
        Vector3 middle = Vector3.Lerp(player.position, npc.position, 0.5f);
        Vector3 dir = Vector3.Cross(between, Vector3.up).normalized;
        
        transform.position = middle + (dir * 2) + Vector3.up*1.5f;
        transform.rotation = Quaternion.identity;
        transform.LookAt(middle);
        _vc.Priority = 20;


    }
}
