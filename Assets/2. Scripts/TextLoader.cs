using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLoader : MonoBehaviour
{
	// CSV 파일에서 읽어온 데이터를 저장하기 위한 Dictionary
    private Dictionary<string, string> _popupTexts;

    private void Awake()
    {
        PopUpTextLoad();
    }
    
    private void PopUpTextLoad()
    {
    	// 데이터 저장을 위한 Dictionary 초기화
        _popupTexts = new Dictionary<string, string>();
            
        // TextAsset : CSV 파일을 담는 변수
        // Resources.Load<TextAsset>() : Resources 폴더에 있는 CSV 파일을 읽어오기 위한 메서드
        // popup_texts : 파일 이름
        TextAsset csvFile = Resources.Load<TextAsset>("popup_texts");
        
        // 해당 파일이 없으면 돌아가기
        if (csvFile == null) return;

		// CSV 파일에 저장된 데이터를 개행(줄바꿈)으로 분리하여 개별 문장으로 저장
        string[] lines = csvFile.text.Split('\n');

		// lines에 저장된 각 문장마다 해당 루틴 실행
        for (int i = 1; i < lines.Length; i++)
        {
        	// 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();
            
            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;
			
            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
			
            // 쉼표로 구분 했을 때 생기는 문장이 2개 이상인 경우(쉼표가 1개 이상인 경우)
            if (parts.Length >= 2)
            {
            	// 첫 번째는 'ID'
                string id = parts[0];
                
                // 두 번째는 'Text'
                string text = parts[1];
				
                // 세 번째 그 이상부터는 CSV 파일에 저장된 데이터에 따라 다르지만
                // 현재 파일에는 id, text 밖에 없기 때문에 세 번째 이상은 모두 text로 해석
                if (parts.Length > 2)
                {
                	// 다음 문장들을 text에 추가
                    for (int j = 2; j < parts.Length; j++)
                    {
                        text += "," + parts[j];
                    }
                }
                // id를 key, text를 data로 묶어 Dictionary에 저장
                _popupTexts[id] = text;
            }
        }
    }

	// 외부에서 데이터를 가져오기 위한 메서드
    // id : CSV 파일의 ID, Dictionary의 Key
    public string GetPopupText(string id)
    {
        return _popupTexts[id];
    }
}
