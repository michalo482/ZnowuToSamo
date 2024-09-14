using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Transform parentCraftSlot;

    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;

    //[SerializeField] private List<UI_CraftSlot> craftSlots;
    
    // Start is called before the first frame update
    void Start()
    {
        //AssignCraftSlot();
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    /*private void AssignCraftSlot()
    {
        for (int i = 0; i < parentCraftSlot.childCount; i++)
        {
            craftSlots.Add(parentCraftSlot.GetChild(i).GetComponent<UI_CraftSlot>());
        }
    }*/

    public void SetupCraftList()
    {
        for (int i = 0; i < parentCraftSlot.childCount; i++)
        {
            Destroy(parentCraftSlot.GetChild(i).gameObject);
        }
        

        //craftSlots = new List<UI_CraftSlot>();

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, parentCraftSlot);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}
