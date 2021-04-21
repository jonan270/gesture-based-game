using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Multitarget")]
public class AbilityMultitarget : AbilityData
{
    /// <summary>
    /// How far away can the enemy be and still be hit
    /// </summary>
    public int attackRange;
    /// <summary>
    /// How far away can a nearby enemy be to the one being hit
    /// </summary>
    public int cleaveRange;

    public override void ActivateAbility()
    {
        Debug.Log("Waiting for player to select a target");
        PlayerManager.Instance.SubscribeToSelectTargetCharacter(OnSelectedCharacter);
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.chooseEnemyCharacter);
    }

    private void OnSelectedCharacter(Character target)
    {
        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();
        List<Character> nearbyEnemies = GetNearbyCharacters(target);

        float cleaveDamage = 0;
        if (targetType == TargetType.single)
            cleaveDamage = powerValue;
        else if(targetType == TargetType.multi)
            cleaveDamage = powerValue / nearbyEnemies.Count;

        Debug.LogError(cleaveDamage);
        //foreach(var enemy in nearbyEnemies)
        //{
        //    float bonusDamage = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().CompareElement(enemy, cleaveDamage, bonusPowerMultiplier);
        //    float damage = bonusDamage + cleaveDamage;

        //    //Debug.LogError("Cast cleave on " + enemy.name + " damaging it for " + damage + " health");

        //    //AbilityManager.ManagerInstance.DamageCharacter(enemy, damage);
        //    AbilityManager.ManagerInstance.CastProjectile(me, target, damage, gestureType);

        //}
        AbilityManager.ManagerInstance.CastProjectile(me, nearbyEnemies, cleaveDamage, gestureType);

        PlayerManager.Instance.UnsubscribeFromSelectTargetCharacter(OnSelectedCharacter);
        AbilityCompleted();
    }

    private List<Character> GetNearbyCharacters(Character target)
    {
        List<Character> nearby = new List<Character>();
        nearby.Add(target);
        foreach(var enemy in PlayerManager.Instance.enemyCharacters)
        {
            if (enemy == target)
                continue;

            float dist = TileDistance(target, enemy);
            if (dist <= cleaveRange)
                nearby.Add(enemy);
        }
        return nearby;
    }

    private float TileDistance(Character first, Character second)
    {
        int x1 = first.CurrentTile.tileIndex.x;
        int y1 = first.CurrentTile.tileIndex.y;
        int x2 = second.CurrentTile.tileIndex.x;
        int y2 = second.CurrentTile.tileIndex.y;
        float dist = Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        return dist;
    }
}
