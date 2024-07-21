using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.AwardCoin();
        Destroy(gameObject);
    }
}
