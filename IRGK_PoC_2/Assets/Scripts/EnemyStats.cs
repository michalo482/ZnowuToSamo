using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy _enemy;
    private ItemDrop _itemDrop;
    public Stat soulsDropAmount;
    

    [Header("Level info")] 
    [SerializeField] private int level = 1;

    [Range(0f, 1f)] 
    [SerializeField] private float percentageModifier = 0.4f;
    
    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        ApplyModifiers();

        base.Start();
        
        _enemy = GetComponent<Enemy>();
        _itemDrop = GetComponent<ItemDrop>();
    }

    private void ApplyModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intellect);
        Modify(vitality);
        
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        
        Modify(maxHp);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
        
        Modify(soulsDropAmount);
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        //enemy.DamageEffects();
    }

    protected override void Die()
    {
        base.Die();
        _enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        _itemDrop.GenerateDrop();
        
        Destroy(gameObject, 5f);
    }

    private void Modify(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
}
