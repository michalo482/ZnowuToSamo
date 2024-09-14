using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string itemDescription;
    public virtual void ExecuteEffect(Transform enemyPosition)
    {
        
    }
}
