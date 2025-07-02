using UnityEngine;

public class CubeTalkTest : MonoBehaviour
{
    [SerializeField] int dialogueStartId = 200; // Inspector에서 직접 대사 ID 지정 가능


    private void OnMouseDown()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogueFromId(dialogueStartId);
            Debug.LogWarning("DialogueManager Instance");
        }
        else
        {
            Debug.LogWarning("Instance가 씬에 없습니다!");
        }
    }
}