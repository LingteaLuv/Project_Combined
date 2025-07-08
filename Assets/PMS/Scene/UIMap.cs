using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    public RawImage fogImage;
    public Transform player;
    public int _Radius = 50;

    private Texture2D fogTexture; 
    private Color[] fogPixels;      ////Texture2D의 모든 픽셀 색상을 저장하는 배열

    void Start()
    {
        // 검은 텍스처 생성
        fogTexture = new Texture2D(512, 512);
        fogPixels = new Color[512 * 512];

        // 전체를 검은색으로 초기화
        for (int i = 0; i < fogPixels.Length; i++)
            fogPixels[i] = Color.black;

        fogTexture.SetPixels(fogPixels);    //픽셀 데이터 변경
        fogTexture.Apply();                 //적용
        fogImage.texture = fogTexture;      //해당 텍스트 이미지 캐싱 -> 나중에 픽셀값을 변하게 하기 위해서
    }

    void Update()
    {
        RevealFog(player.position); // 플레이어 좌표를 UI 텍스쳐 좌표로 변환하여 
    }

    void RevealFog(Vector3 worldPos)
    {
        // 월드 좌표를 텍스처 좌표로 변환 0,0을 중심으로 512,512크기ui지도가 있으면 반지름이 256임, 플레이어가 0,0이면 UI상에서는 해당 256,256이 지도의 중심점 이기 때문에 
        int x = Mathf.RoundToInt(worldPos.x + 256);
        int y = Mathf.RoundToInt(worldPos.z + 256);

        //플레이어 위치 기준으로 원형으로 Pixel을 탐색해서 해당 Pixel의 컬러값을 clear
        // 원형으로 안개 제거
        for (int i = -_Radius; i <= _Radius; i++)
        {
            for (int j = -_Radius; j <= _Radius; j++)
            {
                if (i * i + j * j <= _Radius * _Radius)         //원의 방정식 원의 중심이 a,b일때 원의 좌표(x,y)        
                {                                               //(x-a)²-(y-b)² = Radius² //지금은 원의 중심이 0,0이기 때문에 x²+y²은 r²이다(반지름) 
                    int pixelX = x + i;                         //그렇기에 if문은 원안의 좌표를 for문 돌릴 수 있음
                    int pixelY = y + j;

                    if (pixelX >= 0 && pixelX < 512 && pixelY >= 0 && pixelY < 512)
                    {
                        fogPixels[pixelY * 512 + pixelX] = Color.clear; //2차원 좌표(x, y)를 1차원 배열 인덱스로 변환 총 전체 
                    }
                }
            }
        }

        fogTexture.SetPixels(fogPixels);
        fogTexture.Apply();
    }
}
