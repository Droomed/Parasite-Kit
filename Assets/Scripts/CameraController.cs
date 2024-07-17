using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Collider2D player;
    void Start()
    {
        
    }
    
    void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
