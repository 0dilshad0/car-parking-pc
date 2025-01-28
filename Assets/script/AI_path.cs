using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_path : MonoBehaviour
{
    public Color LineColor;
    private List<Transform> node = new List<Transform>();

     void OnDrawGizmos()
    {
        Gizmos.color = LineColor;

        Transform[] pathTransforn = GetComponentsInChildren<Transform>(); 
        node = new List<Transform>();

        for(int i = 0; i <pathTransforn.Length;i++)
        {
            if (pathTransforn[i]!=transform)
            {
                node.Add(pathTransforn[i]);
            }
        }
        for(int i=0;i<node.Count;i++)
        {
            Vector3 currentNode = node[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i>0)
            {
                previousNode = node[i- 1].position;
            }
            else if(i==0 && node.Count>1)
            {
                previousNode = node[node.Count - 1].position;
            }
            Gizmos.DrawLine(currentNode, previousNode);
            Gizmos.DrawSphere(currentNode, 0.3f);
        }

    }
}
