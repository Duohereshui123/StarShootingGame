using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] AudioData fullHpPickUpSFX;
    [SerializeField] int fullHpBonusScore = 200;
    [SerializeField] float hpBonus = 20f;
    protected override void PickUp()
    {
        if (player.IsFullHp)
        {
            pickUpSFX = fullHpPickUpSFX;
            lootMessage.text = $"Score + {fullHpBonusScore}";
            ScoreManager.Instance.AddScore(fullHpBonusScore);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"HP + {hpBonus}";
            player.RestoreHp(hpBonus);
        }
        base.PickUp();
    }
}

