using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Berserk")]
public class Berserk : AbilityData
{
    public override void OnHit(GameObject target, GameObject attacker)
    {
        target.GetComponent<Character>().ModifyHealth(-amount);

        Instantiate(effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

}
