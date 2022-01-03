using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class NPCController : MonoBehaviour
{
    private const int LeftMouseButton = 0;

    private void MoveToMousePosition()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MoveAgentToPoint(hit.point);
            }
            else
            {
                return;
            }           
        }
    }

    private void MoveAgentToPoint(Vector3 destination) 
    {
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent))
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    private void Update()
    {
        MoveToMousePosition();
    }

}
