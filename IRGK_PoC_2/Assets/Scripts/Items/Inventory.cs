using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour, ISaveManager
{
    
    public static Inventory instance;

    public List<ItemData> startingEquipment;
    
    [FormerlySerializedAs("inventoryItems")] public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")] 
    [SerializeField] private Transform statSlotParent;
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] _inventoryItemSlots;
    private UI_ItemSlot[] _stashItemSlots;
    private UI_EquipmentSlot[] _equipmentSlots;
    private UIStatSlot[] _statSlots;
 
    [Header("Items cooldown")] 
    private float _armorCooldown;
    private float _lastTimeUsedFlask;
    private float _lastTimeUsedArmor;
    public float _flaskCooldown { get; private set; }

    [Header("Database")] 
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;
    
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        _statSlots = statSlotParent.GetComponentsInChildren<UIStatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }
        
        
        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }
        
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if (startingEquipment[i] != null)
            {
                AddItem(startingEquipment[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(ItemData item)
    {
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null; 

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> stuff in equipmentDictionary)
        {
            if (stuff.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = stuff.Key;    
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        
        RemoveItem(item);
        
        UpdateUISlot();
    }

    public void UnequipItem(ItemData_Equipment itemToDelete)
    {
        if (equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToDelete);
            itemToDelete.RemoveModifiers();
        }
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> stuff in equipmentDictionary)
            {
                if (stuff.Key.equipmentType == _equipmentSlots[i].slotType)
                {
                    _equipmentSlots[i].UpdateSlot(stuff.Value);    
                }
            }
        }
        
        for (int i = 0; i < _inventoryItemSlots.Length; i++)
        {
            _inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < _stashItemSlots.Length; i++)
        {
            _stashItemSlots[i].CleanUpSlot();
        }
        
        
        for (int i = 0; i < inventory.Count; i++)
        {
            _inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            _stashItemSlots[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < _statSlots.Length; i++)
        {
            _statSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData item)
    {

        if (item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(item);
        }
        else if (item.itemType == ItemType.Material)
        {
            AddToStash(item);
        }
        
        UpdateUISlot();
    }

    private void AddToStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        
        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }
        
        UpdateUISlot();
    }

    public bool CanBeCrafted(ItemData_Equipment itemToCraft, List<InventoryItem> requiredMaterial)
    {

        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        
        for (int i = 0; i < requiredMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterial[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterial[i].stackSize)
                {
                    Debug.Log("nie mam materialow");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);    
                }
            }
            else
            {
                Debug.Log("nie mam materialow");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
            Debug.Log("usunalem " + materialsToRemove[i].data.name);
        }
        
        AddItem(itemToCraft);
        Debug.Log("zrobilem " + itemToCraft.name);
        
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType type)
    {
        ItemData_Equipment equippedItemData = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> stuff in equipmentDictionary)
        {
            if (stuff.Key.equipmentType == type)
            {
                equippedItemData = stuff.Key;
            }
        }

        return equippedItemData;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > _lastTimeUsedFlask + _flaskCooldown;

        if (canUseFlask)
        {
            _flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ExecuteItemEffect(PlayerManager.instance.player.transform);
            _lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("flask na cd");
        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > _lastTimeUsedArmor + _armorCooldown)
        {
            _armorCooldown = currentArmor.itemCooldown;
            _lastTimeUsedArmor = Time.time;
            return true;
        }
        Debug.Log("zbroja na cd");
        return false;
    }

    public bool CanAddItem()
    {
        if (inventory.Count >= _inventoryItemSlots.Length)
        {
            return false;
        }

        return true;
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, int> pair in data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item)
                    {
                        stackSize = pair.Value
                    };

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in data.equipmentId)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inventory.Clear();
        data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            data.equipmentId.Add(pair.Key.itemId);
        }
    }
    #if UNITY_EDITOR
    [ContextMenu("Uzupelanianie bazy danych itemow")]
    private void FillUpItemDataBase()
    {
        itemDataBase = new List<ItemData>(GetItemDataBase());
    }
    
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Items/SOItems" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
    #endif
}
