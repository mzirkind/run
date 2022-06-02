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

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _agent.SetDestination(_target.position);
        }

        if (_agent.stoppingDistance > _agent.remainingDistance)
        {
            _agent.ResetPath();
        }
    }
    
}
