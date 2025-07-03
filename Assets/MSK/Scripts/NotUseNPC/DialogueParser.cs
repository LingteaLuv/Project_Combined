using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // 회화 리스트
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);    //  Csv파일 가져오기

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            Debug.Log(data[i]);
            if (++i < data.Length) {; }
        }
        return dialogueList.ToArray();
    }
    private void Start()
    {
        Parse("TestDialogue");
    }
}
