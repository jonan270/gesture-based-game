using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private List<GameObject> CharacterList = new List<GameObject>();
    private List<AbilityData> BjornAbilities = new List<AbilityData>();
    private List<AbilityData> HildaAbilities = new List<AbilityData>();

    void Start()
    {
        CharacterList = GameObject.Find("Game Manager").GetComponent<CharacterControl>().listOfCharacters;
        Debug.Log(CharacterList.Count);
        for (int i = 0; i < CharacterList.Count; i++)
        {
            /*if (listOfCharacters[i].GetComponent<Character>().Name == "Bjorn")
                BjornAbilities = listOfCharacters[i].GetComponent<Character>().ListAbilityData;

            if (listOfCharacters[i].GetComponent<Character>().Name == "Hilda")
                HildaAbilities = listOfCharacters[i].GetComponent<Character>().ListAbilityData;*/
        }

        //Default Attack
            HildaAbilities[0].OnHit(CharacterList[0], CharacterList[1]); // Instead of CharacterList[0]: GetTarget(enemy)

        //Debug.Log("Hildas första ability: " + HildaAbilities[0].abilityName);
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
        /*for(int i = 0; i < listOfAbilities.Count; i++)
        {
            //if (BjornAbilities[i].GetType() == typeof(Poison))
            //{
                //Debug.Log("Here");
                //BjornAbilities[i].Apply(GetTarget());
            //}
        }*/
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

}

    //Maybe do abilities like this: https://answers.unity.com/questions/1727492/spells-and-abilities-system.html


