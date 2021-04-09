using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedEffect : MonoBehaviour
{
    public float healthMod;
    public float maxHealthMod;
    public float attackMod;

    public int turnCount;


    public void setTurnBased(Character character, float hMod, float aMod, float maxMod, int turns)
    {
        //isActive = true;
        healthMod = hMod;
        attackMod = aMod;
        maxHealthMod = maxMod;
        turnCount = turns;

        character.attackValue *= (int)attackMod;
        character.maxHealth *= maxHealthMod;
        character.currentHealth *= maxHealthMod;

        ApplyTurnBased(character); // Activate effect right away
    }

    public void ApplyTurnBased(Character character)
    {

        Debug.Log("Turn based effect doing turn based things on " + character.name);
        Debug.Log("Current health: " + character.currentHealth + ", Max health: " + character.maxHealth);
        character.ModifyHealth(healthMod);
        turnCount--;
    }

    public bool IsActive()
    {
        if (turnCount > 0)
            return true;
        else
            return false;
    }

    private void RemoveTurnBased(Character character)
    {
        character.attackValue /= (int)attackMod;
        character.currentHealth /= maxHealthMod;
        
        //healthMod = 0;
        //attackMod = 1;
        //maxHealthMod = 1;
    }
}
