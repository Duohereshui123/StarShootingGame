using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
    [SerializeField] AudioData fullPowerPickUpSFX;
    [SerializeField] int fullPowerBonusScore = 200;
    protected override void PickUp()
    {
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"Score + {fullPowerBonusScore}";
            ScoreManager.Instance.AddScore(fullPowerBonusScore);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"Power Up";
            player.PowerUp();
        }
        base.PickUp();
    }
}
