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
    /// When the user has selected an enemy character cast a fireball on that character
    /// </summary>
    /// <param name="target"></param>
    private void OnSelectedCharacter(Character target)
    {
        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        float bonusDamage = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().CompareElement(target, powerValue, bonusPowerMultiplier);
        float damage = bonusDamage + powerValue;

        List<Character> targetList = new List<Character>();
        targetList.Add(target);

        AbilityManager.ManagerInstance.CastProjectile(me, targetList, damage, gestureType);

        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);
        AbilityCompleted();
    }
}
