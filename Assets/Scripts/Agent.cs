using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 position;

	void Update ()
    {
        transform.position = position;
	}
}
