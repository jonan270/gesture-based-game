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
        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        float totalHeal = powerValue + CalculateBonusHeal(me);

        Debug.Log("Healing " + target.name + " for " + totalHeal + " amount of health");
        target.ModifyHealth(totalHeal);
        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);        
        AbilityCompleted();
    }

    /// <summary>
    /// Bonus heal if user stands in a tile with the same element as this ability has. eg FireHealAbility and FireTile = bonus heal
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float CalculateBonusHeal(Character target)
    {
        float bonus = 1f;
        if (target.CurrentTile.tileType == abilityElement)
            bonus = bonusPowerMultiplier;

        return powerValue * (bonus - 1);
    }
}
