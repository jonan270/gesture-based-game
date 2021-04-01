using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DefaultAttack")]
public class DefaultAttack : AbilityData
{

    public override void OnHit(GameObject target, GameObject attacker)
    {
        amount = attacker.GetComponent<Character>().CompareEnemyElement(target.GetComponent<Character>().Element, extraPowerElement);

        target.GetComponent<Character>().ModifyHealth(-amount);
    }

    public int AutoAttack(int damage)
    {
        return -damage;
    }
}
