using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FreyrTrap")]
public class FreyrTrap : AbilityData
{
    //private const ElementState freyrElement = ElementState.Earth;
    //private const int baseDamage = 30;

    public override void ActivateAbility()
    {

        PlayerManager.Instance.SubscribeToSelectTargetTile(TileReceived);
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.chooseTile);
    }

    private void TileReceived(Hextile tile)
    {
        Debug.Log("Damage: " + CalculateBonusDamage(tile));
        AbilityManager.ManagerInstance.PlaceAreaEffect(tile.tileIndex.x, tile.tileIndex.y, true, abilityElement, CalculateBonusDamage(tile));
        PlayerManager.Instance.UnsubscribeFromSelectTargetTile(TileReceived);
        AbilityCompleted();
    }

    private int CalculateBonusDamage(Hextile tile)
    {
        if (tile.tileType == abilityElement)
            return powerValue * 2;
        else if (tile.tileType == ElementState.Fire)
            return powerValue / 2;
        else
            return powerValue;
    }
}
