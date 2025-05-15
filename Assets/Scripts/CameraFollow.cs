using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform player;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y + yOffset, -10);
        }
    }
}
