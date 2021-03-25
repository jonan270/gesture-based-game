using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heal")]
public class Heal : dmgOrHealAbility
{
    public override void OnHit(Character target, Character caster)
    {
        //If(friendly target)
        target.ModifyHealth(amount);

        Instantiate(effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

}
