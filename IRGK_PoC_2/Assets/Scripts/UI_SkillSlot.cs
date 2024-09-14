using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{

    private UI _ui;
    public bool unlocked;

    [SerializeField] private int skillPrice;
    
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private Color lockedSkillColor;
    

    [SerializeField] private UI_SkillSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillSlot[] shouldBeLocked;

    [SerializeField] private Image skillImage;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    // Start is called before the first frame update
    void Start()
    {
        skillImage = GetComponent<Image>();
        _ui = GetComponentInParent<UI>();
        skillImage.color = lockedSkillColor;

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    public void UnlockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughCurrency(skillPrice) == false)
        {
            return;
        }
        
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.skillTooltip.ShowTooltip(skillDescription, skillName, skillPrice);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.skillTooltip.HideTooltip();
    }

    public void LoadData(GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            data.skillTree.Remove(skillName);
            data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            data.skillTree.Add(skillName, unlocked);
        }
    }
}
