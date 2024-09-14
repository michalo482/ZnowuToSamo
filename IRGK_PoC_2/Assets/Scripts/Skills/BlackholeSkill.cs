using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{

    [SerializeField] private UI_SkillSlot blackholeUnlockButton;
    public bool blackholeUnlocked;
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;

    private BlackholeSkillController currentBlackhole;


    private void UnlockBlackHole()
    {
        if (blackholeUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
        }
    }
    
    protected override void Start()
    {
        base.Start();
        
        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position + new Vector3(0,0, 0.5f), Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackhole)
        {
            return false;
        }
        
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    protected override void CheckUnlocked()
    {
        UnlockBlackHole();
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
