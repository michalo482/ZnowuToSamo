using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialImages;
    [SerializeField] private Button craftButton;

    public void SetupCraftWindow(ItemData_Equipment data)
    {
        
        craftButton.onClick.RemoveAllListeners();
        for (int i = 0; i < materialImages.Length; i++)
        {
            materialImages[i].color = Color.clear;
            materialImages[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < data.craftingMaterials.Count; i++)
        {
            if (data.craftingMaterials.Count > materialImages.Length)
            {
                Debug.Log("za dużo materialow, za malo slotow");
            }

            materialImages[i].sprite = data.craftingMaterials[i].data.icon;
            materialImages[i].color = Color.white;
                
            materialImages[i].GetComponentInChildren<TextMeshProUGUI>().text = data.craftingMaterials[i].stackSize.ToString();
            materialImages[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            
        }

        itemIcon.sprite = data.icon;
        itemName.text = data.itemName;
        itemDescription.text = data.GetDescription();
        
        craftButton.onClick.AddListener(() => Inventory.instance.CanBeCrafted(data, data.craftingMaterials));
    }

}
