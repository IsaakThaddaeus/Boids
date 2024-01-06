using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector2 pos;
    public Vector2 velocity;

    public void updatePosition(){
        transform.position = new Vector3(pos.x, pos.y, 0);
        transform.up = velocity;
    }
}
