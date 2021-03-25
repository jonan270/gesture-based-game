using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum EffectType
{
    AoeDamage,
    SingleTargetDamage,
    SingleTargetHeal
}*/

public class AbilityManager : MonoBehaviour
{
    //public List<AbilityData> ListAbilityData = new List<AbilityData>();
    private List<AbilityData> HildaAbilities;
    private List<AbilityData> BjornAbilities;

    void Start()
    {
        HildaAbilities = GameObject.Find("Hilda 1(Clone)").GetComponent<Character>().ListAbilityData;
        BjornAbilities = GameObject.Find("Bjorn(Clone)").GetComponent<Character>().ListAbilityData;

        Debug.Log("Hildas första ability: " + HildaAbilities[0].abilityName);
    }

    void Update()
    {
        //if next round
        calculateBuffs();
        //Check if card has been chosen
        
    }
    public void triggerAbility(List<AbilityData> listAbilityData)
    {
        for (int i = 0; i < listAbilityData.Count; i++)
        {
          if(listAbilityData[i].abilityName == "Heal")
            {
                //listAbilityData[i].OnHit(GetTarget(), GetCaster());
            }
        }
    }

    public void calculateBuffs()
    {
        for(int i = 0; i < BjornAbilities.Count; i++)
        {
            if (BjornAbilities[i].GetType() == typeof(Poison))
            {
                //Debug.Log("Here");
                //BjornAbilities[i].Apply(GetTarget());
            }
        }
    }

    public void Tick(int nrTurns)
    {

        //nrTurns -= 1;

       /* if (nrTurns <= 0) // Have this in AbilityManager / CharacterControl??
        {
            // End();
            //IsFinished = true;
        }*/
    }


    /*public void chooseAbility(string gesture)
    {
        for(int i = 0; i < ListAbilityData.Count; i++)
        {
           // ListAbilityData[i].TriggerAbility();
        }
    }*/
}

    //Maybe do abilities like this: https://answers.unity.com/questions/1727492/spells-and-abilities-system.html

    /*public void TriggerAbility(Character target, Character attacker) //If gesture is correct
    {
        //int totDmg = data.abilityValue + attacker.CompareEnemyElement(target.Element, extraPowerElement);
        
        if (data.abilityName == "Heal")
        {
            Heal(target);

        }else if(data.abilityName == "DrinkMead")
        {
            //DrinkMead(attacker);
        } else
        {
            //if (data.targType == TargetType MULTI)
            //{
            //    totDmg = totDmg / 3;
                //Get attackers' CurrentTile and calculate nearby tiles => see if any Enemy chars are on those tiles
            //}

            //target.GetComponent<Character>().ModifyHealth(-totDmg);

            //Instantiate(data.effectPrefab, new Vector3(0, 0, 0), Quaternion.identity); //Instantiate(data.effectPrefab, attacker.pos, attacker.rot);
        }

        
    }*/

    /*public void Heal(Character target)
    {
        target.ModifyHealth(data.abilityValue);

        Instantiate(data.effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void Buff(Character attacker)
    {
        target.attackValue += 5;

        //Get all teammates attackValue and buff their values.

        Instantiate(data.effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
    }*/





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



