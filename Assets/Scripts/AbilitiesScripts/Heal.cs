using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heal")]
public class Heal : AbilityData
{
    public override void OnHit(GameObject target, GameObject attacker)
    {
        //If(friendly target)
        target.GetComponent<Character>().ModifyHealth(amount);

        Instantiate(effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

}
