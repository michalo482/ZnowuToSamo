using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterAnimationTriggers : MonoBehaviour
{
    private EnemyBigMonster enemyBigMonster => GetComponentInParent<EnemyBigMonster>();

    private void AnimationTrigger()
    {
        enemyBigMonster.AnimationFinishTrigger();
    }
    
    private void AttackTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(enemyBigMonster.attackCheck.position, enemyBigMonster.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemyBigMonster.Stats.DoDamage(target);
                //hit.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterWindow() => enemyBigMonster.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemyBigMonster.CloseCounterAttackWindow();
}
