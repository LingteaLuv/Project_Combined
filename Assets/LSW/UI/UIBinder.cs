using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBinder : Singleton<UIBinder>
{
    [Header("Drag&Drop")] 
    // RecipeSetting이 들어있는 오브젝트(content) 드래그 가져오기
    // Inventory_Crafting - Crafting - Content - Scroll View - ViewPort - Content
    [SerializeField] private RecipeSetting _craftingUI;

    [SerializeField] private PopUpUI _popUpUI;
    
    public RecipeSetting GetCraftingUI()
    {
        return _craftingUI;
    }

    public void GetInventory(Dictionary<int,int> countById)
    {
        _craftingUI.GetCurrentCount(countById);
    }

    public PopUpUI GetPopupUI()
    {
        return _popUpUI;
    }
}
