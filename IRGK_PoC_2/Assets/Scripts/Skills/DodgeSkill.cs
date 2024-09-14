using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")] 
    [SerializeField] private UI_SkillSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage Dodge")] 
    [SerializeField] private UI_SkillSlot unlockMirageDodgeButton;
    public bool mirageDodgeUnlocked;

    protected override void Start()
    {
        base.Start();
        
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            dodgeUnlocked = true;
            //PlayerManager.instance.player.Stats.evasion.AddModifier(evasionAmount);
            player.Stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
        {
            mirageDodgeUnlocked = true;
        }
    }

    protected override void CheckUnlocked()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    public void CreateMirageOnDodge()
    {
        if (mirageDodgeUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.Anim.transform, Vector3.zero);
        }
    }
    
}
