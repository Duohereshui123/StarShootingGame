using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] AudioData launchSFX;
    int currentAmount;
    bool isMissileReady = true;

    private void Awake()
    {
        currentAmount = defaultAmount;
    }
    private void Start()
    {
        MissileDisplay.UpdateAmountText(currentAmount);
    }

    public void PickUp()
    {
        currentAmount++;
        MissileDisplay.UpdateAmountText(currentAmount);

        if (currentAmount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isMissileReady = true;
        }
    }
    public void Launch(Transform muzzleTransform)
    {
        if (currentAmount == 0 || !isMissileReady) return;

        isMissileReady = false;
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        currentAmount--;
        MissileDisplay.UpdateAmountText(currentAmount);

        if (currentAmount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(CooldownCoroutine());
        }

        IEnumerator CooldownCoroutine()
        {
            var cooldownValue = cooldownTime;
            while (cooldownValue > 0)
            {
                MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
                cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0);

                yield return null;
            }
            isMissileReady = true;
        }
    }
}
