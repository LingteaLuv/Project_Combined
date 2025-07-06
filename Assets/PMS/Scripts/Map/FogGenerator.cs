using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogGenerator : MonoBehaviour
{
    /*[SerializeField] private GameObject[] _fogPrefabs;

    [Header("맵 설정")]
    public Vector3 startPosition = new Vector3(265.5f, 270f, 162.5f);  // 시작 ZX 좌표
    public Vector3 endPosition = new Vector3(-184.5f, 270, -287.5f);      // 끝 ZX 좌표

    [Header("큐브 설정")]
    public GameObject cubePrefab;  // 큐브 프리팹 (없으면 기본 큐브 생성)
    public Material fogMaterial;
    public int cubesPerAxisX = 1000; // X축 방향 큐브 개수
    public int cubesPerAxisZ = 1000; // Z축 방향 큐브 개수
    public float cubeHeight = 1f;  // 큐브 높이

    private GameObject[] fogCubes;
    private Transform parentTransform;


    private void Start()
    {
        GenerateFogs();
    }

    /// <summary>
    /// 안개 오브젝트들을 생성하는 메인 함수
    /// </summary>
    public void GenerateFogs()
    {
        // 부모 오브젝트 생성
        GameObject parent = new GameObject("FogCubes");
        parentTransform = parent.transform;

        // 총 큐브 개수
        int totalCubes = cubesPerAxisX * cubesPerAxisZ;
        fogCubes = new GameObject[totalCubes];

        // 맵 크기 계산
        float mapWidth = endPosition.x - startPosition.x;
        float mapDepth = endPosition.z - startPosition.z;

        // 각 큐브의 크기 계산
        float cubeScaleX = mapWidth / cubesPerAxisX;
        float cubeScaleZ = mapDepth / cubesPerAxisZ;

        int cubeIndex = 0;

        for (int x = 0; x < cubesPerAxisX; x++)
        {
            for (int z = 0; z < cubesPerAxisZ; z++)
            {
                // 큐브 위치 계산
                float posX = startPosition.x + (x + 0.5f) * cubeScaleX;
                float posZ = startPosition.z + (z + 0.5f) * cubeScaleZ;
                float posY = startPosition.y + cubeHeight * 0.5f;

                Vector3 cubePosition = new Vector3(posX, posY, posZ);

                // 큐브 생성
                GameObject cube = CreateCube(cubePosition, cubeScaleX, cubeScaleZ);
                cube.name = $"FogCube_{x}_{z}";
                cube.transform.SetParent(parentTransform);
                cube.AddComponent<BoxCollider>();
                cube.AddComponent<FogCollisionFadeIn>();
                fogCubes[cubeIndex] = cube;
                cubeIndex++;
            }
        }

        Debug.Log($"총 {totalCubes}개의 안개 큐브를 생성했습니다.");
    }

    private GameObject CreateCube(Vector3 position, float scaleX, float scaleZ)
    {
        GameObject cube;

        // 프리팹이 있으면 사용, 없으면 기본 큐브 생성
        if (cubePrefab != null)
        {
            cube = Instantiate(cubePrefab, position, Quaternion.identity);
        }
        else
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
        }

        // 큐브 스케일 조정
        cube.transform.localScale = new Vector3(scaleX, cubeHeight, scaleZ);

        // 재질 적용
        if (fogMaterial != null)
        {
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = fogMaterial;
            }
        }

        return cube;
    }*/


    [Header("맵 설정")]
    public Vector3 startPosition = new Vector3(265.5f, 270f, 162.5f);
    public Vector3 endPosition = new Vector3(-184.5f, 270, -287.5f);

    [Header("안개 설정")]
    public int fogResolution = 1000;  // 텍스처 해상도
    public Material fogMaterial;
    public GameObject player;
    public float viewRadius = 5f;

    private Texture2D fogTexture;
    private Renderer fogRenderer;

    void Start()
    {
        CreateFogTexture();
        CreateFogQuad();
    }

    void CreateFogTexture()
    {
        // 1000x1000 텍스처 생성 (100만개 오브젝트 대신)
        fogTexture = new Texture2D(fogResolution, fogResolution, TextureFormat.R8, false);
        fogTexture.filterMode = FilterMode.Point;

        // 모든 픽셀을 흰색(안개)으로 초기화
        Color[] pixels = new Color[fogResolution * fogResolution];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        fogTexture.SetPixels(pixels);
        fogTexture.Apply();

        fogMaterial.SetTexture("_FogTexture", fogTexture);
    }

    void CreateFogQuad()
    {
        // 안개를 표시할 Quad 생성
        GameObject fogQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        fogQuad.name = "FogQuad";

        // 맵 크기에 맞게 스케일 조정
        Vector3 mapSize = endPosition - startPosition;
        fogQuad.transform.localScale = new Vector3(mapSize.x, mapSize.z, 1f);

        // 위치 조정
        Vector3 center = (startPosition + endPosition) * 0.5f;
        fogQuad.transform.position = new Vector3(center.x, center.y + 0.1f, center.z);
        fogQuad.transform.rotation = Quaternion.Euler(90, 0, 0);

        // 머티리얼 적용
        fogRenderer = fogQuad.GetComponent<Renderer>();
        fogRenderer.material = fogMaterial;
    }

    void Update()
    {
        if (player != null)
        {
            ClearFogAroundPlayer();
        }
    }

    void ClearFogAroundPlayer()
    {
        // 플레이어 위치를 텍스처 좌표로 변환
        Vector3 playerPos = player.transform.position;

        float normalizedX = (playerPos.x - startPosition.x) / (endPosition.x - startPosition.x);
        float normalizedZ = (playerPos.z - startPosition.z) / (endPosition.z - startPosition.z);

        // 범위 체크
        normalizedX = Mathf.Clamp01(normalizedX);
        normalizedZ = Mathf.Clamp01(normalizedZ);

        int texX = Mathf.RoundToInt(normalizedX * fogResolution);
        int texY = Mathf.RoundToInt(normalizedZ * fogResolution);

        Debug.Log($"플레이어 위치: {playerPos}");
        Debug.Log($"정규화된 좌표: ({normalizedX:F2}, {normalizedZ:F2})");
        Debug.Log($"텍스처 좌표: ({texX}, {texY})");

        // 시야 범위만큼 안개 제거
        int radius = Mathf.RoundToInt(viewRadius);

        for (int x = texX - radius; x <= texX + radius; x++)
        {
            for (int y = texY - radius; y <= texY + radius; y++)
            {
                if (x >= 0 && x < fogResolution && y >= 0 && y < fogResolution)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(texX, texY));
                    if (distance <= radius)
                    {
                        fogTexture.SetPixel(x, y, Color.clear);
                    }
                }
            }
        }

        fogTexture.Apply();
    }
}
        /*
        Vector3 playerPos = player.transform.position;

        // 플레이어 위치를 텍스처 좌표로 변환
        float normalizedX = (playerPos.x - startPosition.x) / (endPosition.x - startPosition.x);
        float normalizedZ = (playerPos.z - startPosition.z) / (endPosition.z - startPosition.z);

        // 범위 체크
        normalizedX = Mathf.Clamp01(normalizedX);
        normalizedZ = Mathf.Clamp01(normalizedZ);

        int texX = Mathf.RoundToInt(normalizedX * (fogResolution - 1));
        int texY = Mathf.RoundToInt(normalizedZ * (fogResolution - 1));

        // 텍스처 좌표계에서의 반지름 계산
        float worldRadius = viewRadius;
        float mapWidth = Mathf.Abs(endPosition.x - startPosition.x);
        float mapHeight = Mathf.Abs(endPosition.z - startPosition.z);

        // 텍스처 픽셀 단위로 반지름 변환
        float texRadiusX = (worldRadius / mapWidth) * fogResolution;
        float texRadiusZ = (worldRadius / mapHeight) * fogResolution;
        float texRadius = Mathf.Max(texRadiusX, texRadiusZ); // 더 큰 값 사용

        // 원형 범위 계산
        int searchRadius = Mathf.CeilToInt(texRadius);

        for (int x = texX - searchRadius; x <= texX + searchRadius; x++)
        {
            for (int y = texY - searchRadius; y <= texY + searchRadius; y++)
            {
                if (x >= 0 && x < fogResolution && y >= 0 && y < fogResolution)
                {
                    // 중심점으로부터의 거리 계산
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(texX, texY));

                    // 원형 범위 내에 있으면 안개 제거
                    if (distance <= texRadius)
                    {
                        fogTexture.SetPixel(x, y, Color.clear);
                    }
                }
            }
        }

        fogTexture.Apply();
    }*/

