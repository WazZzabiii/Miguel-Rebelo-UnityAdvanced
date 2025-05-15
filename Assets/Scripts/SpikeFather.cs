using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFather : MonoBehaviour
{
    public HealthBar healthBar;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        Debug.Log("Collided");
        if (player != null)
        {
            OnSpikeCollided();
        }
    }

    protected virtual void OnSpikeCollided()
    {

    }
}
