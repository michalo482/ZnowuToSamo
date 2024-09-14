using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;
    protected Player player;

    private void Awake()
    {
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlocked();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        //Debug.Log("Skill na cd");
        return false;
    }

    public virtual void UseSkill()
    {
        
    }

    protected virtual void CheckUnlocked()
    {
        
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider[] colliders = Physics.OverlapSphere(checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
