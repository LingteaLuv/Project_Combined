using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    [Header("Map Settings")]
    public GameObject _mapUI;
    public RectTransform _mapImage;
    public RectTransform _playerIcon;
    public Transform _player;

    [Tooltip("worldSize -> 2Dmap max x,y")]
    [Header("World Bounds")]
    public Vector2 _worldSize = new Vector2(1000, 1000);
    public Vector2 _worldCenter = Vector2.zero;

    [Header("Icon Settings")]
    [Tooltip("아이콘을 맵 경계에 제한할지 여부")]
    public bool _clampIconToMap = true;
    [Tooltip("맵 경계에서 아이콘까지의 여백 (픽셀)")]
    public float _iconPadding = 10f;
    [Tooltip("플레이어가 맵 밖에 있을 때 아이콘 색상")]
    public Color _outsideMapColor = Color.black;
    [Tooltip("플레이어가 맵 안에 있을 때 아이콘 색상")]
    public Color _insideMapColor = Color.white;

    private bool _isMapOpen = false;
    private Image _playerIconImage;

    private void Start()
    {
        // 플레이어 아이콘의 Image 컴포넌트 가져오기
        _playerIconImage = _playerIcon.GetComponent<Image>();
    }

    private void Update()
    {
        // M키로 지도 토글
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
        // 지도가 열려있으면 플레이어 위치 업데이트
        if (_isMapOpen)
        {
            UpdatePlayerPosition();
        }
    }

    /// <summary>
    /// 토글형식 지도 껏다켜기
    /// </summary>
    public void ToggleMap()
    {
        _isMapOpen = !_isMapOpen;
        _mapUI.SetActive(_isMapOpen);
        if (_isMapOpen)
        {
            UpdatePlayerPosition();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdatePlayerPosition()
    {
        // 월드 좌표를 지도 UI 좌표로 변환
        Vector2 playerWorldPos = new Vector2(_player.position.x, _player.position.z);
        Vector2 normalizedPos = WorldToMapPosition(playerWorldPos);

        //지도의랑 실제 맵이랑 크기가 같지 않기 때문에 플레이어 Pos를 mapImage 비율 만큼 맞춰주는 작업이 필요
        // UI 좌표로 변환 (중심점 기준으로 변환)
        Vector2 mapPos = new Vector2(
            (normalizedPos.x - 0.5f) * _mapImage.rect.width,   // 중심점 기준으로 변환
            (normalizedPos.y - 0.5f) * _mapImage.rect.height   // 중심점 기준으로 변환
        );

        // 플레이어가 맵 경계 안에 있는지 확인
        bool isInsideMap = IsPlayerInsideMapBounds(normalizedPos);

        //아이콘을 밖으로 나갈게 할 것이지 안할것인지 
        if (_clampIconToMap)
        {
            // 아이콘을 맵 경계 내로 제한
            mapPos = ClampIconToMapBounds(mapPos);
        }

        _playerIcon.anchoredPosition = mapPos;

        // 플레이어가 맵 밖에 있으면 아이콘 색상 변경
        UpdateIconAppearance(isInsideMap, normalizedPos);


        // 플레이어 회전도 반영 (선택적)
        // playerIcon.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
        /*// 월드 좌표를 지도 UI 좌표로 변환
        Vector2 playerWorldPos = new Vector2(player.position.x, player.position.z);
        Vector2 normalizedPos = WorldToMapPosition(playerWorldPos);

        // UI 좌표로 변환
        Vector2 mapPos = new Vector2(
            normalizedPos.x * mapImage.rect.width,
            normalizedPos.y * mapImage.rect.height
        );

        // 플레이어가 맵 경계 안에 있는지 확인
        bool isInsideMap = IsPlayerInsideMapBounds(normalizedPos);

        if (clampIconToMap)
        {
            // 아이콘을 맵 경계 내로 제한
            mapPos = ClampIconToMapBounds(mapPos);
        }

        playerIcon.anchoredPosition = mapPos;

        // 플레이어가 맵 밖에 있으면 아이콘 색상 변경
        UpdateIconAppearance(isInsideMap, normalizedPos);

        // 플레이어 회전도 반영 (선택적)
        // playerIcon.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);*/
    }

    private Vector2 WorldToMapPosition(Vector2 worldPos)
    {
        // 월드 좌표를 0~1 범위로 정규화
        Vector2 relativePos = worldPos - _worldCenter;
        Vector2 normalizedPos = new Vector2(
            (relativePos.x + _worldSize.x * 0.5f) / _worldSize.x,
            (relativePos.y + _worldSize.y * 0.5f) / _worldSize.y
        );
        return normalizedPos;
    }

    private bool IsPlayerInsideMapBounds(Vector2 normalizedPos)
    {
        // 정규화된 좌표가 0~1 범위 안에 있는지 확인
        return normalizedPos.x >= 0f && normalizedPos.x <= 1f &&
               normalizedPos.y >= 0f && normalizedPos.y <= 1f;
    }

    //경계선확인 Padding -> 경계선 + @값
    private Vector2 ClampIconToMapBounds(Vector2 mapPos)
    {
        // 맵 이미지의 실제 크기 고려
        float halfWidth = _mapImage.rect.width * 0.5f;
        float halfHeight = _mapImage.rect.height * 0.5f;

        // 아이콘 패딩을 고려한 경계 설정
        float minX = -halfWidth + _iconPadding;
        float maxX = halfWidth - _iconPadding;
        float minY = -halfHeight + _iconPadding;
        float maxY = halfHeight - _iconPadding;

        // 위치를 경계 내로 제한
        return new Vector2(
            Mathf.Clamp(mapPos.x, minX, maxX),
            Mathf.Clamp(mapPos.y, minY, maxY)
        );
    }

    private void UpdateIconAppearance(bool isInsideMap, Vector2 normalizedPos)
    {
        if (_playerIconImage == null) return;

        //Map안에 있는지 없는지에 따라 색변경
        if (isInsideMap)
        {
            _playerIconImage.color = _insideMapColor;
        }
        else
        {
            _playerIconImage.color = _outsideMapColor;
        }
    }
}

