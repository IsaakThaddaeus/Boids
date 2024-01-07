using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Header("Settings")]
    public float numbreOfBoids;
    public GameObject boidObject;
    public int n;
    public LayerMask layerMask;

    public float length;
    public float radius;

    public float width;
    public float height;
    public float depth;
    float topMargin;
    float leftMargin;
    float rightMargin;
    float bottomMargin;
    float frontMargin;
    float backMargin;

    private List<Boid> boids;


    [Header("Parameters")]
    public float turnfactor = 0.2f;
    public float visualRange = 20f;
    public float protectedRange = 2f;
    public float centeringfactor = 0.0005f;
    public float avoidfactor = 0.2f;
    public float matchingfactor = 0.05f;
    public float obstaclefactor = 0.05f;
    public float maxspeed = 3f;
    public float minspeed = 2f;


    void Start()
    {
        boids = new List<Boid>();
        topMargin = height / 2;
        leftMargin = -width / 2;
        rightMargin = width / 2;
        bottomMargin = -height / 2;
        frontMargin = depth / 2;
        backMargin = -depth / 2;


        //Init Boids
        for (int i = 0; i < numbreOfBoids; i++)
        {
            Vector3 initialPosition = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), Random.Range(-depth / 2, depth / 2));
            Debug.Log(initialPosition);

            GameObject boid = Instantiate(boidObject);
            boid.transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
            boid.transform.parent = transform;
            boid.name = "Boid: " + i;

            Boid boidScript = boid.GetComponent<Boid>();
            boidScript.setVisualRange(n, length);
            boidScript.pos = initialPosition;
            boids.Add(boidScript);
        }

    }

    private void Update()
    {
        foreach(Boid boid in boids)
        {
            Vector3 close = Vector3.zero;
            Vector3 pos_avg = Vector3.zero;
            Vector3 vel_avg = Vector3.zero;
            Vector3 avoid = Vector3.zero;
            int neighboring_boids = 0;

            foreach (Boid otherboid in boids)
            {
                float distance = Vector3.Distance(boid.pos, otherboid.pos);

                if(distance < visualRange)
                {
                    if(distance < protectedRange)
                    {
                        close += boid.pos - otherboid.pos;
                    }

                    else if(distance < visualRange)
                    {
                        pos_avg += otherboid.pos;
                        vel_avg += otherboid.velocity;
                        neighboring_boids += 1;
                    }
                }
            }

            RaycastHit hit;
            if (Physics.SphereCast(boid.pos, radius, boid.velocity.normalized, out hit, length, layerMask))
            {
                for(int i = 0; i < boid.visualRange.Count; i++)
                {
                    if (!Physics.SphereCast(boid.pos, radius, boid.visualRange[i], out hit, length, layerMask))
                    {
                        avoid += boid.visualRange[i].normalized;
                        break;
                    }
                }
            }

            if (neighboring_boids > 0)
            {
                pos_avg = pos_avg / neighboring_boids;
                vel_avg = vel_avg / neighboring_boids;
                boid.velocity += (pos_avg - boid.pos) * centeringfactor + (vel_avg - boid.velocity) * matchingfactor;
            }

            boid.velocity += close * avoidfactor;
            boid.velocity += avoid * obstaclefactor;


            if (boid.pos.y > topMargin)
                boid.velocity += new Vector3(0, -turnfactor, 0);

            if(boid.pos.y < bottomMargin)
                boid.velocity += new Vector3(0, turnfactor, 0);

            if (boid.pos.x < leftMargin)
                boid.velocity += new Vector3(turnfactor, 0, 0);

            if (boid.pos.x > rightMargin)
                boid.velocity += new Vector3(-turnfactor, 0, 0);

            if (boid.pos.z > frontMargin)
                boid.velocity += new Vector3(0, 0, -turnfactor);

            if (boid.pos.z < backMargin)
                boid.velocity += new Vector3(0, 0, turnfactor);

            float speed = boid.velocity.magnitude;

            if (speed < minspeed)
                boid.velocity = boid.velocity.normalized * minspeed;

            if (speed > maxspeed)
                boid.velocity = boid.velocity.normalized * maxspeed;

            boid.pos = boid.pos + boid.velocity * Time.deltaTime;
            boid.updatePosition();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, depth));
    }
}
