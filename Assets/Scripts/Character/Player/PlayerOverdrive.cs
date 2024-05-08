using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//ʹ��ί�� ������֮������

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
        //ί������ĳ�Ա���� ���Կ�����awake��destroy�ﶩ�ĺ��˶�
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
