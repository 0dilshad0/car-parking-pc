using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parking : MonoBehaviour
{
   
   
    [SerializeField] ParticleSystem poper;
    [SerializeField] GameObject Car;
    [SerializeField] ParticleSystem zone;
    [SerializeField] GameObject winScreen;

    bool inside = false;
    bool parked = false;
    
    public  bool isInsde => inside;
    //public bool isParked => parked;


    // Start is called before the first frame update
    void Start()
    {
        winScreen.SetActive(false);
        poper.Pause();
        //Debug.Log("Parking script initialized on: " + gameObject.name);
        var zoneArea = zone;
        zoneArea.startColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCar"))
        {
          
            var zoneArea = zone;
            zoneArea.startColor = Color.yellow;
            inside = IsInside(other);
           
            if(inside && !parked)
            {
                Invoke("Park",1.5f);
            }
        }
      
      

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCar"))
        {
            var zoneArea = zone;
            zoneArea.startColor = Color.red;
         
            parked = false;
        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCar"))
        {
            inside = IsInside(other);

            if(isInsde && !parked)
            {
                var zoneArea = zone;
                zoneArea.startColor = Color.green;
                Invoke("Park", 1.5f);
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
          
            parked = true;
            poper.Play(); 
            Debug.Log("Car parked successfully!");
            StartCoroutine(compleat());
        }
    }

    IEnumerator compleat()
    {
        yield return new WaitForSecondsRealtime(1f);
        {
            winScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            unlockLevel();
            Time.timeScale = 0f;
        }
    }

    private void unlockLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >=  PlayerPrefs.GetInt("ReachIndex"))
        {
            PlayerPrefs.SetInt("ReachIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel",PlayerPrefs.GetInt("UnlockedLevel",1)+1);
            PlayerPrefs.Save();
            PlayerPrefs.Save();
        }
    }
   
   
}
