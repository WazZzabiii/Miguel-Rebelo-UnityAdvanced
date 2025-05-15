using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinScript : MonoBehaviour
{
    public UnityEvent coinCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coinCollect.Invoke();
            Destroy (gameObject);   
        }
    }

    public void Test()
    {
        Debug.Log("Coin Collected");
    }
}
