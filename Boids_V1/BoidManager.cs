using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Header("Settings")]
    public float numbreOfBoids;
    public GameObject boidObject;
    public float width;
    public float height;
    float topMargin;
    float leftMargin;
    float rightMargin;
    float bottomMargin;

    private List<Boid> boids;


    [Header("Parameters")]
    public float turnfactor = 0.2f;
    public float visualRange = 20f;
    public float protectedRange = 2f;
    public float centeringfactor = 0.0005f;
    public float avoidfactor = 0.2f;
    public float matchingfactor = 0.05f;
    public float maxspeed = 3f;
    public float minspeed = 2f;


    void Start()
    {
        boids = new List<Boid>();
        topMargin = height / 2;
        leftMargin = -width / 2;
        rightMargin = width / 2;
        bottomMargin = -height / 2;


        //Init Boids
        for (int i = 0; i < numbreOfBoids; i++)
        {
            Vector2 initialPosition = new Vector2(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2));
            Debug.Log(initialPosition);

            GameObject boid = Instantiate(boidObject);
            boid.transform.position = new Vector3(initialPosition.x, initialPosition.y, 0);
            boid.transform.parent = transform;
            boid.name = "Boid: " + i;

            Boid boidScript = boid.GetComponent<Boid>();
            boidScript.pos = initialPosition;
            boids.Add(boidScript);
        }

    }

    private void Update()
    {
        foreach(Boid boid in boids)
        {
            Vector2 close = Vector2.zero;
            Vector2 pos_avg = Vector2.zero;
            Vector2 vel_avg = Vector2.zero;
            int neighboring_boids = 0;

            foreach (Boid otherboid in boids)
            {
                float distance = Vector2.Distance(boid.pos, otherboid.pos);

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

            if (neighboring_boids > 0)
            {
                pos_avg = pos_avg / neighboring_boids;
                vel_avg = vel_avg / neighboring_boids;
                boid.velocity += (pos_avg - boid.pos) * centeringfactor + (vel_avg - boid.velocity) * matchingfactor;
            }

            boid.velocity = boid.velocity + (close * avoidfactor);


            if (boid.pos.y > topMargin)
                boid.velocity += new Vector2(0, -turnfactor);

            if(boid.pos.y < bottomMargin)
                boid.velocity += new Vector2(0, turnfactor);

            if (boid.pos.x < leftMargin)
                boid.velocity += new Vector2(turnfactor, 0);

            if (boid.pos.x > rightMargin)
                boid.velocity += new Vector2(-turnfactor, 0);


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
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 0));
    }
}
