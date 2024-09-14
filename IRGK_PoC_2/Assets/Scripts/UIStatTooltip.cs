using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UIStatTooltip : UI_Tooltip
{

    [SerializeField] private TextMeshProUGUI description;


    public void ShowStatTooltip(string text)
    {
        description.text = text;
        AdjustPosition();
        
        gameObject.SetActive(true);
    }

    public void HideStatTooltip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
