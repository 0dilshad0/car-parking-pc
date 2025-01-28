using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook thredPerson;
    [SerializeField] CinemachineVirtualCamera firsrPerson;
    public GameObject charecter;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        thredPerson.Priority = 11;
        firsrPerson.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(thredPerson.Priority==11)
            {
                charecter.SetActive(false);
                thredPerson.Priority = 0;
                firsrPerson.Priority = 11;
            }
            else
            {
                charecter.SetActive(true);
                thredPerson.Priority = 11;
                firsrPerson.Priority = 0;
            }
        }
    }
}
