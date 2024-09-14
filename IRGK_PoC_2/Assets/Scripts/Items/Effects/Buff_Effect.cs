using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "BuffEffect", menuName = "Data/Item Effect/Buff")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats _stats;
    [SerializeField] private StatsType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        _stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        Debug.Log("zwiekszam sile");
        _stats.IncreaseStatBy(buffAmount, buffDuration, _stats.GetStat(buffType));
    }

    
}
