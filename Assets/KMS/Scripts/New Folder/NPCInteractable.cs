using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum NPCState
{
    Normal,
    Active,
    Complete
}
public class NPCInteractable : MonoBehaviour
{
    [SerializeField] public NPCDialogue Dialogue;

    [SerializeField] private GameObject _ballon;
    [SerializeField] private TMP_Text _ballonText;

    public Property<NPCState> State = new Property<NPCState>(NPCState.Normal);

    private void Start()
    {
        _ballon.SetActive(false);
        State.OnChanged += SetBallonText;
    }

    private void SetBallonText(NPCState s)
    {
        switch (s)
        {
            case NPCState.Normal:
                _ballonText.text = "...";
                break;
            case NPCState.Active:
                _ballonText.text = "?";
                break;
            case NPCState.Complete:
                _ballonText.text = "!";
                break;
        }
    }
    public void OnBallon()
    {
        _ballon.SetActive(true);
    }
    public void OffBallon()
    {
        _ballon.SetActive(false);
    }


}
