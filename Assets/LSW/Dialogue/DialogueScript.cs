using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private WaitForSeconds _delay;
    private bool _skipRequested;
    
    private void OnEnable()
    {
        Init();
        StartCoroutine(ScriptSetting.WriteWords
            (text, "안녕하세요 저는 원숭이라고 합니다.", _delay, () => _skipRequested));
    }

    private void Init()
    {
        // 글자 출력 딜레이 캐싱
        _delay = new WaitForSeconds(0.05f);
    }
    
    public void OnClickSkipButton()
    {
        _skipRequested = true;
    }
}
