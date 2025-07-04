using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSetting : MonoBehaviour
{
    [Header("Drag&Drop")] 
    // ScriptableObjects - New Item Dictionary 드래그
    [SerializeField] private ItemDictionary _itemDictionary;

    [SerializeField] private TextMeshProUGUI _explainText;
    
    private Recipe _recipe;
    
    private Image _resultImage;
    private Image[] _materialImages;
    private TMP_Text[] _materialTexts;
    private TMP_Text[] _currentTexts;
    private Image[] _currentImage;
    
    public List<Button> CreateBtn { get; private set; }
    public Stack<Button> RecipeBtn { get; private set; }

    private bool _hasMaterial1;
    private bool _hasMaterial2;
    private bool _hasMaterial3;
    private bool _hasMaterial4;

    private bool[] _hasMaterials;

    private Dictionary<int, int> _countById;

    private void Awake()
    {
        _materialImages = new Image[4];
        _materialTexts = new TMP_Text[4];
        _currentTexts = new TMP_Text[4];
        _currentImage = new Image[4];
        CreateBtn = new List<Button>();
        RecipeBtn = new Stack<Button>();
        
        SetValueEditor();
    }
    
    public void SetValueEditor()
    {
        _itemDictionary.GenerateDic();

        for (int i = 0; i < _itemDictionary.RecipeDic.Count; i++)
        {
            SetRecipe(i);
            SetProperty();
            MaterialInit(i);
            SetValue();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedUIUpdate());
    }

    public IEnumerator DelayedUIUpdate()
    {
        yield return null;

        for (int i = 0; i < _itemDictionary.RecipeDic.Count; i++)
        {
            SetRecipe(i);
            SetProperty();
            CurrentTextInit(i);
            LinkInventory();
        }
    }
    
    private void LinkInventory()
    {
        if (_hasMaterial1)
        {
            _currentTexts[0].text = _countById[_recipe.MaterialItemId1].ToString();
            if(_countById[_recipe.MaterialItemId1] >= _recipe.MaterialItemQuantity1)
            {
                _currentImage[0].color = Color.white;
            }
            else
            {
                _currentImage[0].color = Color.black;
            }
        }

        if (_hasMaterial2)
        {
            _currentTexts[1].text = _countById[_recipe.MaterialItemId2].ToString();
            if(_countById[_recipe.MaterialItemId2] >= _recipe.MaterialItemQuantity2)
            {
                _currentImage[1].color = Color.white;
            }
            else
            {
                _currentImage[1].color = Color.black;
            }
        }

        if (_hasMaterial3)
        {
            _currentTexts[2].text = _countById[_recipe.MaterialItemId3].ToString();
            if(_countById[_recipe.MaterialItemId3] >= _recipe.MaterialItemQuantity3)
            {
                _currentImage[2].color = Color.white;
            }
            else
            {
                _currentImage[2].color = Color.black;
            }
        }

        if (_hasMaterial4)
        {
            _currentTexts[3].text = _countById[_recipe.MaterialItemId4].ToString();
            if(_countById[_recipe.MaterialItemId4] >= _recipe.MaterialItemQuantity4)
            {
                _currentImage[3].color = Color.white;
            }
            else
            {
                _currentImage[3].color = Color.black;
            }
        }
    }

    public void GetCurrentCount(Dictionary<int,int> countById)
    {
        _countById = new Dictionary<int, int>();
        _countById = countById;
    }
    
    private void SetRecipe(int index)
    {
        _recipe = _itemDictionary.RecipeDic[_itemDictionary.RecipeKeys[index]];
    }
    
    private void SetProperty()
    {
        _hasMaterial1 = _recipe.MaterialItemId1 != 0;
        _hasMaterial2 = _recipe.MaterialItemId2 != 0;
        _hasMaterial3 = _recipe.MaterialItemId3 != 0;
        _hasMaterial4 = _recipe.MaterialItemId4 != 0;

        _hasMaterials = new bool[4] { _hasMaterial1, _hasMaterial2, _hasMaterial3, _hasMaterial4 };
    }

    private Transform[] Init(int index)
    {
        Transform targetTransform = transform.GetChild(index);
        
        Transform[] children = new Transform[6];

        for (int i = 0; i < targetTransform.childCount-3; i++)
        {
            children[i] = targetTransform.GetChild(i + 3);
        }
        
        return children;
    }
    
    private void CurrentTextInit(int index)
    {
        Transform[] children = Init(index);

        for (int i = 0; i < 4; i++)
        {
            if (_hasMaterials[i])
            {
                _currentTexts[i] = children[i + 1].GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
                _currentImage[i] = children[i + 1].GetChild(1).GetComponent<Image>();
            }
        }
    }
    
    private void MaterialInit(int index)
    {
        Transform[] children = Init(index);
        
        Button recipeBtn = transform.GetChild(index).GetChild(0).GetComponent<Button>();
        RecipeBtn.Push(recipeBtn);
        
        _resultImage = children[0].GetChild(0).GetComponentInChildren<Image>();
        CreateBtn.Add(children[5].GetComponentInChildren<Button>());

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
    }
    
    private void SetValue()
    {
        _resultImage.sprite = _itemDictionary.ItemDic[_recipe.ResultItemId].Sprite;
        int resultId = _recipe.ResultItemId;
        RecipeBtn.Peek().onClick.AddListener(() => _explainText.text = _itemDictionary.ItemDic[resultId].Description);

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
