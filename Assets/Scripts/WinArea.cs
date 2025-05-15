using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinArea : MonoBehaviour
{
    [SerializeField] GameObject winText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null )
        {
            winText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
