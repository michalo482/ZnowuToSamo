using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{

    [Header("Dash")] 
    public bool dashUnlocked;
    [SerializeField] private UI_SkillSlot dashButton;

    [Header("Clone on dash")] 
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillSlot cloneDashButton;

    [Header("Clone on arrival")] 
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillSlot cloneOnArrivalDashButton;
    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();
        dashButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlocked()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    private void UnlockDash()
    {
        if (dashButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if (cloneDashButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalDashButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }
    
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.Anim.transform, Vector3.zero);
        }
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.Anim.transform, Vector3.zero);
        }
    }
}
