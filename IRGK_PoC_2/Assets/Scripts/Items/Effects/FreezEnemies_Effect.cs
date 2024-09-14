using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreezeEnemiesEffect", menuName = "Data/Item Effect/Freeze enemies")]
public class FreezEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform transform)
    {
        PlayerStats stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (stats.currentHp > stats.GetMaxHp() * 0.1f)
        {
            return;
        }
        
        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeForTwo(duration);
        }
    }
}
