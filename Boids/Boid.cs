using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 pos;
    public Vector3 velocity;
    public List<Vector3> visualRange;

    public void setVisualRange(int n, float length){
        visualRange = FibonacciLattice.distributePoints(n, length);
    }
    public void updatePosition(){
        transform.position = new Vector3(pos.x, pos.y, pos.z);
        transform.up = velocity;
    }
}
