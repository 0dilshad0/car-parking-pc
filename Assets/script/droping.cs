using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class droping : MonoBehaviour
{


    public PlayableDirector cutseen;
    public GameObject customer;

    bool inside = false;
    bool parked = false;

    public bool isInsde => inside;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCar"))
        {
          

            inside = IsInside(other);

            if (inside && !parked)
            {
                Invoke("Park", 1f);
            }
        }



    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCar"))
        {
         
            parked = false;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCar"))
        {
            inside = IsInside(other);

            if (isInsde && !parked)
            {
                Invoke("Park", 1f);
            }

        }
    }
    private bool IsInside(Collider other)
    {
        Bounds triggerBounds = GetComponent<Collider>().bounds;
        Bounds OtherBounds = other.bounds;
        return triggerBounds.Contains(OtherBounds.min) && triggerBounds.Contains(OtherBounds.max);
    }
    private void Park()
    {
        if (inside)
        {
            customer.SetActive(true);
            cutseen.Play();
            Debug.Log("Car parked successfully!");
        }
    }

}
