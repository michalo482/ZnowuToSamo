using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillSlot parryUnlockButton;
    public bool parryUnlocked;

    [Header("Restorative Parry")] 
    [SerializeField] private UI_SkillSlot restoreUnlockButton;
    public bool restoreUnlocked;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHpAmount;
    
    [Header("Parry with a mirage")] 
    [SerializeField] private UI_SkillSlot parryWithMirageUnlockButton;
    public bool mirageParryUnlocked;
    
    
    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.Stats.GetMaxHp() * restoreHpAmount);
            player.Stats.IncreasHpBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlocked()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            mirageParryUnlocked = true;
        }
    }

    public void MakeMirageOnParry()
    {
        if (mirageParryUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay();
        }
    }
}
