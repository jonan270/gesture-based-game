using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum EffectType
{
    AoeDamage,
    SingleTargetDamage,
    SingleTargetHeal
}*/

public class Ability : MonoBehaviour
{
    public AbilityData data;

    private static int extraPowerElement = 5;

    //Maybe do abilities like this: https://answers.unity.com/questions/1727492/spells-and-abilities-system.html

    public int TriggerAbility(Character target, Character attacker) //If gesture is correct
    {
        int totDmg = data.abilityValue + attacker.CompareEnemyElement(target.Element, 5);

        if (data.healAbility == true)
        {
            Heal(target);

        }else if(data.buffAbility == true)
        {
            Buff(target);
        } else
        {
            if (data.areaAttack == true)
            {
                totDmg = totDmg / 3;
                //Get attackers' CurrentTile and calculate nearby tiles => see if any Enemy chars are on those tiles
            }

            target.GetComponent<Character>().ModifyHealth(-totDmg);

            Instantiate(data.effectPrefab, new Vector3(0, 0, 0), Quaternion.identity); //Instantiate(data.effectPrefab, attacker.pos, attacker.rot);
        }

        return totDmg;
        
    }
    
    public void Heal(Character target)
    {
        target.ModifyHealth(data.abilityValue);

        Instantiate(data.effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void Buff(Character target)
    {
        target.attackValue += 5;

        //Get all teammates attackValue and buff their values.

        Instantiate(data.effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
    }

    /*public class AttackAbility : Ability
    {
        public int TriggerAbility()
        {

        }
    }

    public class AoEAttackAbility : AttackAbility
    {

    }

public class BuffAbility : Ability
    {
        public int TriggerAbility()
        {

        }
    }*/

}


public enum ElementState
{
    Fire, Earth, Water, Wind
}

/*public class BaseAbility : Ability
{
public override void TriggerAbility(GameObject ob)
{

}
}
public class Heal : Ability
{
public int healingPower = 15;
}

public class DrinkMead : Ability
{

}

public class Cleave : Ability
{

}*/



