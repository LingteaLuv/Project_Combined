
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button _slotButton;

    [SerializeField] private Image _hoverImage;

    [SerializeField] private DoubleClickManage _dcm;

    private void Awake()
    {
        _dcm = GetComponentInParent<DoubleClickManage>();
    }

    public void OnEnable()
    {
        HoverImageDisable();
    }
    public void HoverImageEnable()
    {
        _hoverImage.enabled = true;
    }
    public void HoverImageDisable()
    {
        _hoverImage.enabled = false;
    }

    public void TryHold()
    {
        int index = GetSlotIndex();
        InventoryManager.Instance.Controller.HoldItem(index);
    }
    public void TryPut()
    {
        InventoryManager.Instance.Controller.PutItem();
    }
    public void DCdown()
    {
        if (InventoryManager.Instance.Model.InvItems[GetSlotIndex()] == null) return;
        _dcm.StartCo(GetSlotIndex());
    }
    public void DCup()
    {
        _dcm.EndCo();
    }
    public void TrySelect()
    {
        int index = GetSlotIndex();
        InventoryManager.Instance.Controller.SelectSlot(index);
    }
    protected int GetSlotIndex()
    {
        int.TryParse(gameObject.name, out int index);
        return index;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryManager.Instance.Controller.IsHolding)
            InventoryManager.Instance.Controller.NextIndex = GetSlotIndex();
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        if (InventoryManager.Instance.Controller.IsHolding)
            InventoryManager.Instance.Controller.NextIndex = InventoryManager.Instance.Controller.HoldingIndex;
    }
    public void DCexit()
    {
        _dcm.IsExist = false;
    }
}
