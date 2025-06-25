using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSetting : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private ItemDictionary _itemDictionary;
    
    private Recipe _recipe;
    
    private Image _resultImage;
    private Image[] _materialImages;
    private TMP_Text[] _materialTexts;
    private Button _createBtn;

    private bool _hasMaterial1;
    private bool _hasMaterial2;
    private bool _hasMaterial3;
    private bool _hasMaterial4;

    private bool[] _hasMaterials;

    private void Awake()
    {
        SetValueEditor();
    }
    
#if UNITY_EDITOR
    
    public void SetValueEditor()
    {
        _itemDictionary.GenerateDic();
        
        for (int i = 0; i < _itemDictionary.RecipeDic.Count; i++)
        {
            SetRecipe(i);
            SetProperty();
            Init(i);
            SetValue();
        }
    }
#endif

    private void SetProperty()
    {
        _hasMaterial1 = _recipe.MaterialItemId1 != 0;
        _hasMaterial2 = _recipe.MaterialItemId2 != 0;
        _hasMaterial3 = _recipe.MaterialItemId3 != 0;
        _hasMaterial4 = _recipe.MaterialItemId4 != 0;

        _hasMaterials = new bool[4] { _hasMaterial1, _hasMaterial2, _hasMaterial3, _hasMaterial4 };
        
        _materialImages = new Image[4];
        _materialTexts = new TMP_Text[4];
    }
    
    private void Init(int index)
    {
        Transform targetTransform = transform.GetChild(index);
        
        Transform[] children = new Transform[6];

        for (int i = 0; i < targetTransform.childCount-2; i++)
        {
            children[i] = targetTransform.GetChild(i + 2);
        }
        _resultImage = children[0].GetChild(0).GetComponentInChildren<Image>();

        for (int i = 0; i < 4; i++)
        {
            if (_hasMaterials[i])
            {
                _materialImages[i] = children[i+1].GetChild(1).GetComponent<Image>();
                _materialTexts[i] = children[i+1].GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
            }
            else
            {
                children[i+1].gameObject.SetActive(false);
            }
        }
        _createBtn = children[5].GetComponentInChildren<Button>();
    }

    private void SetRecipe(int index)
    {
        _recipe = _itemDictionary.RecipeDic[index + 9001];
    }
    
    private void SetValue()
    {
        _resultImage.sprite = _itemDictionary.ItemDic[_recipe.ResultItemId].Sprite;

        if (_hasMaterial1)
        {
            _materialImages[0].sprite = _itemDictionary.ItemDic[_recipe.MaterialItemId1].Sprite;
            _materialTexts[0].text = _recipe.MaterialItemQuantity1.ToString();
        }

        if (_hasMaterial2)
        {
            _materialImages[1].sprite = _itemDictionary.ItemDic[_recipe.MaterialItemId2].Sprite;
            _materialTexts[1].text = _recipe.MaterialItemQuantity2.ToString();
        }

        if (_hasMaterial3)
        {
            _materialImages[2].sprite = _itemDictionary.ItemDic[_recipe.MaterialItemId3].Sprite;
            _materialTexts[2].text = _recipe.MaterialItemQuantity3.ToString();
        }

        if (_hasMaterial4)
        {
            _materialImages[3].sprite = _itemDictionary.ItemDic[_recipe.MaterialItemId4].Sprite;
            _materialTexts[3].text = _recipe.MaterialItemQuantity4.ToString();
        }
    }
}
