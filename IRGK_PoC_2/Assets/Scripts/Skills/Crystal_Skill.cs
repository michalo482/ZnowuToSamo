using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject _currentCrystal;

    [Header("Crystal Mirage")] 
    [SerializeField] private UI_SkillSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal Simple")] 
    [SerializeField] private UI_SkillSlot unlockCrystalButton;
    public bool crystalUnlocked;

    [Header("Explosive Crystal")] 
    [SerializeField] private UI_SkillSlot unlockExplosiveCrystalButton;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")] 
    [SerializeField] private UI_SkillSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")] 
    [SerializeField] private UI_SkillSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
        
    }

    protected override void CheckUnlocked()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();
    }

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveCrystalButton.unlocked)
        {
            canExplode = true;
        }
    }

    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
        {
            canUseMultiStack = true;
        }
    }
    
    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        Vector3 offset = new Vector3(0, 1, 0);
        
        if (_currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }
            Vector3 playerPos = player.transform.position + offset;
            player.transform.position = _currentCrystal.transform.position - offset;
            _currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(_currentCrystal.transform, new Vector3(0, -1, 0));
                Destroy(_currentCrystal);
            }
            else
            {
                _currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        Vector3 offset = new Vector3(0, 1, 0);

        Vector3 playerPosition = player.transform.position + offset;
        
        _currentCrystal = Instantiate(crystalPrefab, playerPosition, Quaternion.identity);
        CrystalSkillController currentCrystalScript = _currentCrystal.GetComponent<CrystalSkillController>();
            
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(_currentCrystal.transform));
        
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        _currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStack)
        {
            if (crystalsLeft.Count > 0)
            {
                if (crystalsLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }
                cooldown = 0;
                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalsLeft.Remove(crystalToSpawn);
                
                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalsLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }

        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalsLeft.Count;
        
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
