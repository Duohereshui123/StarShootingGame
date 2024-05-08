using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] float speed = 360f;
    [SerializeField] Vector3 angle;

    private void OnEnable()
    {
        StartCoroutine(nameof(RotateCoroutine));
    }

    IEnumerator RotateCoroutine()
    {
        while (true)
        {
            transform.Rotate(angle * speed * Time.deltaTime);
            yield return null;
        }
    }
}
