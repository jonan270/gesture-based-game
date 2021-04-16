using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedEffect : MonoBehaviour
{
    public float healthMod;
    public float defMod;
    public float attackMod;

    public int turnCount;

    //[SerializeField]
    public GameObject visualEffect;

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
        //visualEffect = character.ListAbilityData[0].effectPrefab;
        //isActive = true;
        healthMod = hMod;
        attackMod = aMod;
        defMod = dMod;
        turnCount = turns;

        character.attackMultiplier *= attackMod;
        character.defenceMultiplier *= defMod;


        visualizeAbility(character, true);
        ApplyTurnBased(character); // Activate effect right away
    }

    /// <summary>
    /// Apply the turnbased effect on a character
    /// </summary>
    /// <param name="character">character to apply effect on</param>
    public void ApplyTurnBased(Character character)
    {
        if (IsActive())
        {
            Debug.Log("Turn based effect doing turn based things on " + character.name);
            Debug.Log("Current health: " + character.CurrentHealth + ", Defensemultiplier: " + character.defenceMultiplier);
            character.ModifyHealth(healthMod);
            turnCount--;
        }
        else
            RemoveTurnBased(character);
    }

    /// <summary>
    /// Check if this effect should be active
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        if (turnCount > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Remove this turnbased effect. TODO: Maybe rework to remove a specific turnbased ability? Works for now.
    /// </summary>
    /// <param name="character"> character to remove turnbased effect from </param>
    public void RemoveTurnBased(Character character)
    {
        character.attackMultiplier /= attackMod;
        character.defenceMultiplier /= defMod;

        visualizeAbility(character, false);
    }

    public void visualizeAbility(Character target, bool show)
    {
        //Character abilityUser = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();
        Transform parent = target.transform;
        if (visualEffect != null && !show)
        {
            target.activeEffect = null;
            Destroy(visualEffect);
        }
        else
        {
            visualEffect = Instantiate(visualEffect, parent.position, Quaternion.identity);
            visualEffect.transform.localScale = parent.localScale;

            visualEffect.transform.SetParent(parent, true);
            visualEffect.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

    }
}
