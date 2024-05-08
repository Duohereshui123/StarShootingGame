using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//使用委托 减少类之间的耦合

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };

    [SerializeField] GameObject triggerVFX;
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXOverdrive;

    [SerializeField] AudioData onSFX;
    [SerializeField] AudioData offSFX;

    void Awake()
    {
        //委托是类的成员变量 所以可以在awake和destroy里订阅和退订
        on += On;
        off += Off;
    }

    void OnDestroy()
    {
        on -= On;
        off -= Off;
    }
    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }
    void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
