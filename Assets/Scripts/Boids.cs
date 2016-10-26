using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boids : MonoBehaviour
{
    public GameObject boidPrefab;
    public uint numberOfBoids = 25;
    private List<Agent> boidsList;

    [Range(0.0f, 1.0f)]
    public float cohesionMod;

    [Range(0.0f, 1.0f)]
    public float dispersionMod;

    [Range(0.0f, 1.0f)]
    public float alignmentMod;

    private void Start()
    {
        boidsList = new List<Agent>();
        initialisePosition();
	}
	
	private void Update()
    {
        moveBoids();
	}

    private void initialisePosition()
    {
        for(int i = 0; i < numberOfBoids; i++)
        {
            GameObject boids = Instantiate(boidPrefab);
            boids.GetComponent<Agent>().position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            boidsList.Add(boids.GetComponent<Agent>());
        }
    }

    private void moveBoids()
    {
        Vector3 v1, v2, v3, v4, v5;
        foreach(Agent allBoids in boidsList)
        {
            v1 = Cohesion(allBoids);
            v2 = Dispersion(allBoids);
            v3 = Alignment(allBoids);
            //v4 = bindPosition(allBoids);
            v5 = tendTowardsPlace(allBoids);

            allBoids.velocity += v1 + v2 + v3 + v5; //+ v4 + v5;
            limitVelocity(allBoids);
            allBoids.position += allBoids.velocity.normalized;
        }
    }

    private Vector3 Cohesion(Agent thisBoid)
    {
        Vector3 perceivedCenter = Vector3.zero;
        foreach(Agent otherBoid in boidsList)
        {
            if (otherBoid != thisBoid)
            {
                perceivedCenter += otherBoid.position;
            }
        }
        perceivedCenter = perceivedCenter / (numberOfBoids - 1);

        return (perceivedCenter - thisBoid.position).normalized * cohesionMod;
    }

    private Vector3 Dispersion(Agent thisBoid)
    {
        Vector3 displacement = Vector3.zero;
        foreach(Agent otherBoid in boidsList)
        {
            if (otherBoid != thisBoid)
            {
                if ((otherBoid.position - thisBoid.position).magnitude < 100)
                {
                    displacement -= otherBoid.position - thisBoid.position;
                }
            }
        }

        return displacement.normalized * dispersionMod;
    }

    private Vector3 Alignment(Agent thisBoid)
    {
        Vector3 percievedVelocity = Vector3.zero;
        foreach (Agent otherBoid in boidsList)
        {
            if (otherBoid != thisBoid)
            {
                percievedVelocity += otherBoid.velocity;
            }
        }
        percievedVelocity = percievedVelocity / (numberOfBoids - 1);

        return ((percievedVelocity - thisBoid.velocity).normalized / 8) * alignmentMod;
    }

    private Vector3 tendTowardsPlace(Agent thisBoid)
    {
        Vector3 place = Vector3.zero;

        return (place - thisBoid.position) / 100;
    }

    private Vector3 bindPosition(Agent thisBoid)
    {
        float xMin = -5;
        float xMax = 5;
        float yMin = -5;
        float yMax = 5;
        float zMin = -5;
        float zMax = 5;
        Vector3 pushBack = Vector3.zero;

        if (thisBoid.position.x < xMin)
        {
            pushBack.x = 3;
        }
        else if (thisBoid.position.x > xMax)
        {
            pushBack.x = -3;
        }
        if (thisBoid.position.y < yMin)
        {
            pushBack.y = 3;
        }
        else if (thisBoid.position.y > yMax)
        {
            pushBack.y = -3;
        }
        if (thisBoid.position.z < zMin)
        {
            pushBack.x = 3;
        }
        else if (thisBoid.position.z > zMax)
        {
            pushBack.x = -3;
        }

        return pushBack.normalized;
    }

    private void limitVelocity(Agent thisBoid)
    {
        float limiter = 15;

        if (thisBoid.velocity.magnitude > limiter)
        {
            thisBoid.velocity = (thisBoid.velocity / thisBoid.velocity.magnitude).normalized * limiter; 
        }
    }
}
