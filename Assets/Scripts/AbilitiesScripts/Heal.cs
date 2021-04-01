using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heal")]
public class Heal : AbilityData
{
    //public override void OnHit(GameObject target, GameObject attacker)
    //{
    //    //If(friendly target)
    //    target.GetComponent<Character>().ModifyHealth(amount);

    //    Instantiate(effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    //}

    public override void ActivateAbility()
    {
        // TODO: Find character to heal and get tileindex x, y ..
        int x = 4;
        int y = 4;

        // TODO: Calculate healing amount ..
        int heal = 15;

        AbilityManager.ManagerInstance.HealCharacter(x, y, heal);

        Debug.Log("ZAPP COME GET HEALS");
        //Character character;
        //character
    }

}
