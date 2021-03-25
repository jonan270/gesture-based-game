using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Poison")]
public class Poison : Buff
{
    public override void Apply(Character target)
    {
        target.GetComponent<Character>().ModifyHealth(-amount);

        Instantiate(effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //Tick();
    }


}