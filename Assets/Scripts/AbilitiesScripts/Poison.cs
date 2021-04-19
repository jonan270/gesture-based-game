using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Poison")]
public class Poison : AbilityData
{
    //public override void OnHit(GameObject target, GameObject attacker)
    //{
    //    target.GetComponent<Character>().ModifyHealth(-amount);
    //}
    public override void ActivateAbility()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Waiting for player to select a target");
        PlayerManager.Instance.SubscribeToSelectTargetCharacter(OnSelectedCharacter);
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.chooseEnemyCharacter);
    }

    private void OnSelectedCharacter(Character target)
    {
        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        Debug.Log("doing " + -powerValue + " in damage");
        AbilityManager.ManagerInstance.ActivateTurnBasedAbility(target, -powerValue, 1f, 1f, 3, gestureType, me);

        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);
        AbilityCompleted();
    }

}