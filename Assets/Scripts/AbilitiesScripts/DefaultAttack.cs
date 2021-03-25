using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DefaultAttack")]
public class DefaultAttack : dmgOrHealAbility
{

    public override void OnHit(Character target, Character attacker)
    {
        amount = attacker.CompareEnemyElement(target.Element, extraPowerElement);

        target.GetComponent<Character>().ModifyHealth(-amount);
    }
}
