using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VictimRoutine : MonoBehaviour
{
    [System.Serializable] public struct RoutineAction
    {
        public Transform target;
        public string bubbleText;
        public string action;
        public float time;
    }

    public List<RoutineAction> routine = new List<RoutineAction>();

    NavMeshAgent nmAgent;
    
    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        nmAgent.destination = routine[0].target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
