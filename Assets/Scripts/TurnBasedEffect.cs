using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedEffect : MonoBehaviour
{
    public float healthMod;
    public float defMod;
    public float attackMod;

    public int turnCount;


    /// <summary>
    /// Set the turn based effect onto character
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hMod"></param>
    /// <param name="aMod"></param>
    /// <param name="dMod"></param>
    /// <param name="turns"></param>
    public void setTurnBased(Character character, float hMod, float aMod, float dMod, int turns)
    {
        //isActive = true;
        healthMod = hMod;
        attackMod = aMod;
        defMod = dMod;
        turnCount = turns;

        character.attackMultiplier *= attackMod;
        character.defenceMultiplier *= defMod;
        //character.currentHealth *= defMod;

        ApplyTurnBased(character); // Activate effect right away
    }

    public void ApplyTurnBased(Character character)
    {
        if (IsActive())
        {
            Debug.Log("Turn based effect doing turn based things on " + character.name);
            Debug.Log("Current health: " + character.currentHealth + ", Defensemultiplier: " + character.defenceMultiplier);
            character.ModifyHealth(healthMod);
            turnCount--;
        }
        else
            RemoveTurnBased(character);
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
        character.attackValue /= attackMod;
        character.currentHealth /= defMod;
    }
}
