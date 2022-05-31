using System;
using UnityEngine;
using UnityEngine.AI;

public class HunterController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;

    private void Start()
    {
        _agent.SetDestination(_target.position);
    }
}
