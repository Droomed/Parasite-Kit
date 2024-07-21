using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private TextMeshProUGUI coins;
    // Start is called before the first frame update
    void Awake()
    {
        coins = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        coins.text = "Coins: " + GameManager.instance.GetCoins();
    }
}
