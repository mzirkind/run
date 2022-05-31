using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TargetController : MonoBehaviour
{
    [SerializeField] private float maxDistanceFromCenter = 3f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            transform.position = GetRandomPoint(Vector3.zero, maxDistanceFromCenter);
        }
    }


    public static Vector3 GetRandomPoint(Vector3 center, float maxDistance)
    {
        NavMeshHit hit;
        do
        {
            Vector3 randomPosition = Random.onUnitSphere * maxDistance + center;
            NavMesh.SamplePosition(randomPosition, out hit, maxDistance, NavMesh.AllAreas);
        } while (!hit.hit);

        return hit.position;
    }
}
