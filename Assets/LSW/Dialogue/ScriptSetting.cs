using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public static class ScriptSetting
{
    // 한 글자씩 출력되는 기능을 코루틴으로 구현
    public static IEnumerator WriteWords
        (TextMeshProUGUI text, string str, WaitForSeconds delay, Func<bool> skipRequested)
    {
        // 최적화를 위한 stringbuilder 활용
        StringBuilder strText = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            strText.Append(str[i]);
            text.text = strText.ToString();
            
            // 외부 입력을 통해 skiprequest 업데이트 시 한 번에 출력
            if (skipRequested())
            {
                text.text = str;
                break;
            }
            yield return delay;
        }
    }
}
