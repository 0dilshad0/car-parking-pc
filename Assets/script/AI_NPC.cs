using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_NPC : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject path;
    public Transform[] point;
    private int index = 0;
    private float minDistence=1f;

    // Start is called before the first frame update
    void Start()
    {
        point = new Transform[path.transform.childCount];
        agent = GetComponent<NavMeshAgent>();
        for(int i=0;i<point.Length;i++)
        {
            point[i] = path.transform.GetChild(i); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, point[index].position) < minDistence) 
        {
            index = (index + 1) % point.Length;
        }
        agent.SetDestination(point[index].position);
    }
}
