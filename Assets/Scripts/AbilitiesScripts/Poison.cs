using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Poison")]
public class Poison : AbilityData
{
    public override void OnHit(GameObject target, GameObject attacker)
    {
        target.GetComponent<Character>().ModifyHealth(-amount);
    }


}