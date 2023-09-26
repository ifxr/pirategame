using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    private int gold;
    private Animator anim;

    public static AudioClip coinPickup;
    static AudioSource source;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerShip player = other.attachedRigidbody.gameObject.GetComponent<PlayerShip>();
        if(player != null)
        {
            coinPickup = Resources.Load<AudioClip>("CoinPickup");
            source = GetComponent<AudioSource>();
            source.Play();
            player.gold += Mathf.RoundToInt(gold * player.GetPickupGoldMultiplier());
            gold = 0;
            anim.SetTrigger("PickedUp");
        }
    }

    private void DestroyPickup()
    {
        Destroy(gameObject);
    }

    public void SetGold(int _gold)
    {
        gold = _gold;
    }
}
