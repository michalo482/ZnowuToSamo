using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;

    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTooltip(ItemData_Equipment item)
    {
        if (item == null)
        {
            return;
        }
        
        itemNameText.text = item.name;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescriptionText.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize *= 0.7f;
        }
        else
        {
            itemNameText.fontSize = 40;
        }
        
        AdjustPosition();
        
        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}
