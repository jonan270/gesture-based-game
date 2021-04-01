using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heal")]
public class Heal : AbilityData
{
    /// <summary>
    /// Activates the heal ability on a friendly character
    /// </summary>
    public override void ActivateAbility()
    {
        Debug.Log("Waiting for player to select a target");
        PlayerManager.Instance.SubscribeToSelectTargetCharacter(OnSelectedCharacter);
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.chooseFriendlyCharacter);
    }

    /// <summary>
    /// When the user has selected a friendly character cast a heal on that character
    /// </summary>
    /// <param name="target"></param>
    private void OnSelectedCharacter(Character target)
    {
        Debug.Log("Healing " + target.name);
        target.ModifyHealth(powerValue + CalculateBonusHeal(PlayerManager.Instance.selectedCharacter.GetComponent<Character>()));
        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);        
        AbilityCompleted();
    }

    /// <summary>
    /// Bonus heal if target stands in a tile with the same element as this ability has. eg FireHealAbility and FireTile = bonus heal
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private int CalculateBonusHeal(Character target)
    {
        int bonus = 0;
        if (target.CurrentTile.tileType == abilityElement)
            bonus = bonusPowerValue;

        return bonus;
    }
}
