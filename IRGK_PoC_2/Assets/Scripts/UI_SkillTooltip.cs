using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{

    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTooltip(string skillDescription, string skillName, int price)
    {
        this.skillName.text = skillName;
        skillText.text = skillDescription;
        skillCost.text = "Cost: " + price;
        
        AdjustPosition();
        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}
