using UnityEngine;

public class CubeTalkTest : MonoBehaviour
{
    [SerializeField] int dialogueStartId = 200; // Inspector에서 직접 대사 ID 지정 가능

    private void OnMouseDown()
    {
        DialogueManager.Instance.StartDialogueFromId(dialogueStartId);
    }
}