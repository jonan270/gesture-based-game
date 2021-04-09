using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedEffect : MonoBehaviour
{
    public bool isActive = false;

    public int healthMod;
    public float maxHealthMod;
    public float attackMod;

    private int turnCount;


    public void setTurnBased(Character character, int hMod, float aMod, float maxMod, int turns)
    {
        isActive = true;
        healthMod = hMod;
        attackMod = aMod;
        maxHealthMod = maxMod;
        turnCount = turns;

        character.basicAttackValue *= (int)attackMod;
        character.maxHealth *= maxHealthMod;
        character.currentHealth *= maxHealthMod;
        ApplyTurnBased(character); // Activate effect right away
    }

    public void ApplyTurnBased(Character character)
    {
        if(isActive)
        {
            if (turnCount > 0)
            {
                Debug.Log("Turn based effect doing turn based things on " + character.name);
                Debug.Log("Current health: " + character.currentHealth + ", Max health: " + character.maxHealth);
                character.ModifyHealth(healthMod);
                turnCount--;
            }
            else
            {
                RemoveTurnBased(character);
            }
        }
    }

    private void RemoveTurnBased(Character character)
    {
        character.basicAttackValue /= (int)attackMod;
        character.currentHealth /= maxHealthMod;
        isActive = false;
        //healthMod = 0;
        //attackMod = 1;
        //maxHealthMod = 1;
    }
}
