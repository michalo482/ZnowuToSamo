using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private string statName;

    [SerializeField] private StatsType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private UI _ui;
    
    [TextArea]
    [SerializeField] private string statDescription;


    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        UpdateStatValueUI();

        _ui = GetComponentInParent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if (statType == StatsType.MaxHp)
            {
                statValueText.text = playerStats.GetMaxHp().ToString();
            }

            if (statType == StatsType.Damage)
            {
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            }

            if (statType == StatsType.CritPower)
            {
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            }

            if (statType == StatsType.CritChance)
            {
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
            }

            if (statType == StatsType.Evasion)
            {
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            }

            if (statType == StatsType.MagicResistance)
            {
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intellect.GetValue() * 3))
                    .ToString();
            }
            
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.statTooltip.HideStatTooltip();
    }
}
