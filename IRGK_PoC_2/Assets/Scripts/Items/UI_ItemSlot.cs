using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;

    public InventoryItem item;

    protected UI _ui;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        
        _itemImage.color = Color.white;
        if (item != null)
        {
            _itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                _itemText.text = item.stackSize.ToString();
            }
            else
            {
                _itemText.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanUpSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
        
        _ui.itemTooltip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        _ui.itemTooltip.ShowTooltip(item.data as ItemData_Equipment);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        
        _ui.itemTooltip.HideTooltip();
    }
}
