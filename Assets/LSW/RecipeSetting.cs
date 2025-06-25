using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSetting : MonoBehaviour
{
    [Header("Drag&Drop")]
    [SerializeField] private Recipe _recipe;

    private Image _resultImage;
    
    private Image _material1Image;
    private Image _material2Image;
    private Image _material3Image;
    private Image _material4Image;

    private TMP_Text _material1Text;
    private TMP_Text _material2Text;
    private TMP_Text _material3Text;
    private TMP_Text _material4Text;

    private Button _createBtn;

#if UNITY_EDITOR
    public void SetValueEditor()
    {
        Init();
        SetValue();
    }
#endif
    
    private void Init()
    {
        Transform[] children = new Transform[6];

        for (int i = 0; i < transform.childCount-2; i++)
        {
            children[i] = transform.GetChild(i + 2);
        }

        _resultImage = children[0].GetComponentInChildren<Image>();
       
        _material1Image = children[1].GetChild(1).GetComponent<Image>();
        _material2Image = children[2].GetChild(1).GetComponent<Image>();
        _material3Image = children[3].GetChild(1).GetComponent<Image>();
        _material4Image = children[4].GetChild(1).GetComponent<Image>();
        
        _material1Text = children[1].GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        _material2Text = children[2].GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        _material3Text = children[3].GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        _material4Text = children[4].GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        _createBtn = children[5].GetComponentInChildren<Button>();
    }

    private void SetRecipe(int index)
    {
        
    }
    
    private void SetValue()
    {
        _resultImage.sprite = ItemDictionary.Instance.ItemDic[_recipe.ResultItemId].Sprite;
        
        _material1Image.sprite = ItemDictionary.Instance.ItemDic[_recipe.MaterialItemId1].Sprite;
        _material2Image.sprite = ItemDictionary.Instance.ItemDic[_recipe.MaterialItemId2].Sprite;
        _material3Image.sprite = ItemDictionary.Instance.ItemDic[_recipe.MaterialItemId3].Sprite;
        _material4Image.sprite = ItemDictionary.Instance.ItemDic[_recipe.MaterialItemId4].Sprite;

        _material1Text.text = _recipe.MaterialItemQuantity1.ToString();
        _material2Text.text = _recipe.MaterialItemQuantity2.ToString();
        _material3Text.text = _recipe.MaterialItemQuantity3.ToString();
        _material4Text.text = _recipe.MaterialItemQuantity4.ToString();
    }
}
