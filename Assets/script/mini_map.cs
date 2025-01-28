using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mini_map : MonoBehaviour
{
    [SerializeField] Transform car;

    private void LateUpdate()
    {
        Vector3 newposs = car.position;
        newposs.y = transform.position.y;
        transform.position = newposs;

        transform.rotation =  Quaternion.Euler(90f, car.eulerAngles.y, 0f);
    }
}
