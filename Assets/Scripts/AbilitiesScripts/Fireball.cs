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
        // *** TEMP ***
        //Transform parent = target.transform;
        //GameObject visualEffect = Instantiate(effectPrefab, parent.position, Quaternion.identity);
        //visualEffect.transform.localScale = parent.localScale;

        //visualEffect.transform.SetParent(parent, true);
        //visualEffect.transform.localEulerAngles = new Vector3(0, 0, 0);
        // *** TEMP ***

        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        float bonusDamage = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().CompareElement(target, powerValue, bonusPowerMultiplier);
        float damage = bonusDamage + powerValue;

        AbilityManager.ManagerInstance.CastProjectile(me, target, damage);

        //Debug.Log("Cast a fireball at " + target.name + " damaging it for " + damage + " health");
        //target.ModifyHealth(damage);
        //AbilityManager.ManagerInstance.DamageCharacter(target, damage);
        
        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);
        AbilityCompleted();
    }
}
