using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalSkillController : MonoBehaviour
{
    private Animator _animator => GetComponent<Animator>();
    private SphereCollider _cd => GetComponent<SphereCollider>();
    private Player _player => PlayerManager.instance.player;
    
    private bool _canGrow;
    [SerializeField] private float growSpeed;
    
    private float _crystalExistTimer;
    private bool _canExplode;
    private bool _canMoveToEnemy;
    private float _moveSpeed;

    private Transform _closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMoveToEnemy, float moveSpeed, Transform closestTarget)
    {
        _crystalExistTimer = crystalDuration;
        _canExplode = canExplode;
        _canMoveToEnemy = canMoveToEnemy;
        _moveSpeed = moveSpeed;
        _closestTarget = closestTarget;
    }

    private void Update()
    {
        _crystalExistTimer -= Time.deltaTime;
        
        if (_crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (_canMoveToEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, _closestTarget.position, _moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _closestTarget.position) < 1)
            {
                FinishCrystal();
                _canMoveToEnemy = false;
            }
        }

        if (_canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(5, 5), growSpeed * Time.deltaTime);
        }
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackHoleRadius();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            _closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _cd.radius);
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //hit.GetComponent<Enemy>().DamageEffects();
                hit.GetComponent<Enemy>();
                _player.Stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                //hit.GetComponent<Enemy>().DamageEffects();
            }
        }
    }

    public void FinishCrystal()
    {
        if (_canExplode)
        {
            _canGrow = true;
            _animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
