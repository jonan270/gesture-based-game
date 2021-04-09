using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Fireball")]
public class Fireball : AbilityData
{
    public override void ActivateAbility()
    {
        Debug.Log("Waiting for player to select a target");
        PlayerManager.Instance.SubscribeToSelectTargetCharacter(OnSelectedCharacter);
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.chooseEnemyCharacter);
    }

    /// <summary>
    /// When the user has selected a friendly character cast a heal on that character
    /// </summary>
    /// <param name="target"></param>
    private void OnSelectedCharacter(Character target)
    {
        float bonusDamage = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().CompareElement(target, powerValue, bonusPowerMultiplier);
        float damage = bonusDamage + powerValue;
        Debug.Log("Cast a fireball at " + target.name + " damaging it for " + damage + " health");
        //target.ModifyHealth(damage);
        AbilityManager.ManagerInstance.DamageCharacter(target, damage);
        
        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);
        AbilityCompleted();
    }
}
