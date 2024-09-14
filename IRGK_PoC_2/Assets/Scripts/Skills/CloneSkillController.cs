//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float colorLoosingSpeed;
    private Animator anim;
    private float cloneTimer;

    private SkinnedMeshRenderer smr;
    private float _attackMultiplayer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    
    
    
    private Transform closestEnemy;
    private bool _canDuplicateClone;
    private int _facingDirection = 1;
    private float _chanceToDuplicate;

    private Player _player;

    private void Awake()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            Color color = smr.material.color;
            color.a = Mathf.Max(0, color.a - (Time.deltaTime * colorLoosingSpeed));
            smr.material.color = color;
            if (smr.material.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 _offset, Player player, Transform _closestEnemy, bool canDuplicate, float chanceToDuplicate, float attackMultiplayer)
    {
        if (canAttack)
        {
            anim.SetInteger("AttackNumber", 1);
        }

        _attackMultiplayer = attackMultiplayer;
        _player = player;
        transform.position = newTransform.position + _offset;
        closestEnemy = _closestEnemy;
        _canDuplicateClone = canDuplicate;
        _chanceToDuplicate = chanceToDuplicate;
        //transform.rotation = newTransform.rotation;
        FaceClosestTarget();
        cloneTimer = cloneDuration;
    }
    
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                
                hit.GetComponent<Entity>().SetupKnockBackDirection(transform);
                //_player.Stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                //hit.GetComponent<Enemy>().DamageEffects();
                PlayerStats playerStats = _player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                
                playerStats.CloneDoDamage(enemyStats, _attackMultiplayer);

                if (_player.skill.clone.canApplyOnHitEffects)
                {
                    ItemData_Equipment itemDataEquipment = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (itemDataEquipment != null)
                    {
                        itemDataEquipment.ExecuteItemEffect(hit.transform);
                    }
                }
                
                if (_canDuplicateClone)
                {
                    if (Random.Range(0, 100) < _chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(2 * _facingDirection, -2, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x < closestEnemy.transform.position.x)
            {
                _facingDirection = -1;
                transform.Rotate(0,180,0);
            }
        }
    }
    
}
