using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        //player.DamageEffects();
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHpBy(int _damage)
    {
        base.DecreaseHpBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.ExecuteItemEffect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
        
    }

    public void CloneDoDamage(CharacterStats _targetStats, float attackMultiplayer)
    {
        if (EvasionCheck(_targetStats))
        {
            return;
        }
        int totalDamage = damage.GetValue() + strength.GetValue();

        if (attackMultiplayer > 0)
        {
            totalDamage =Mathf.RoundToInt(totalDamage * attackMultiplayer);
        }
        if (CritCheck())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
        totalDamage = ArmorCheck(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }
}
