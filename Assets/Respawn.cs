using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float threshold;

    private void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            transform.position = new Vector2
}
}
}
