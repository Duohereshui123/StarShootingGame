using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    Material material;

    [SerializeField] private Vector2 scrollVelocity;
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }


    IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }

}
