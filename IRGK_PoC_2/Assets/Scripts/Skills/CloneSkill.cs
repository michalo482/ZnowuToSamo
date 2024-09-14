using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone Info")] 
    [SerializeField] private float attackMultiplayer;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;

    [Header("Clone Attack")] 
    [SerializeField] private UI_SkillSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplayer;
    [SerializeField] private bool canAttack;

    [Header("Aggressive Clone")] 
    [SerializeField] private UI_SkillSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneMultiplayer;
    public bool canApplyOnHitEffects;
    //[SerializeField] private bool createCloneOnDashStart;
    //[SerializeField] private bool createCloneOnDashOver;
    //[SerializeField] private bool canCreateCloneOnCounterAttack;
    [Header("Multiple Clone")] 
    [SerializeField] private UI_SkillSlot multipleCloneUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplayer;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal Instead Of Clone")] 
    [SerializeField] private UI_SkillSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();
        
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>()
            .SetUpClone(clonePosition, cloneDuration, canAttack, offset, player, FindClosestEnemy(newClone.transform), canDuplicateClone,chanceToDuplicate, attackMultiplayer);
    }

    protected override void CheckUnlocked()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockCrystalInstead();
        UnlockMultiClone();
    }

    public void CreateCloneWithDelay()
    {
        
        StartCoroutine(CloneDelayCoroutine());
        
    }

    private IEnumerator CloneDelayCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
            CreateClone(player.Anim.transform, new Vector3(3 * player.FacingDirection, 0, 0));
    }

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplayer = cloneAttackMultiplayer;
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffects = true;
            attackMultiplayer = aggressiveCloneMultiplayer;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleCloneUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplayer = multiCloneAttackMultiplayer;
        }
    }

    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }
}
