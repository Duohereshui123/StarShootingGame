using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 5f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] protected AudioData defaultPickUpSFX;

    protected Player player;

    Animator animator;
    int pickUpStateID = Animator.StringToHash("PickUp");

    protected AudioData pickUpSFX;
    protected Text lootMessage;
    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        pickUpSFX = defaultPickUpSFX;
        lootMessage  = GetComponentInChildren<Text>(true);
    }

    void OnEnable()
    {
        StartCoroutine(MoveCoroutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        StopAllCoroutines();
        animator.Play(pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);
    }
    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 direction = Vector3.left;
        while (true)
        {
            if (player.isActiveAndEnabled)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }
}
