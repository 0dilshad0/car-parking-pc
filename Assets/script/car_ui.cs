using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class car_ui : MonoBehaviour
{
    public int current=0;
    public Scrollbar gear;
    public GameObject D;
    public GameObject R;
    public GameObject pause;
    public GameObject settings;


    private void Start()
    {
        gear.value = 1;
        D.SetActive(true);
        R.SetActive(false);
        pause.SetActive(false);
        settings.SetActive(false);
    }

    private void Update()
    {
        
        if(Input.GetKey(KeyCode.Q))
        {
            gear.value = 1;
            D.SetActive(true);
            R.SetActive(false);
        }
        if(Input.GetKey(KeyCode.E))
        {
            gear.value = 0;
            D.SetActive(false);
            R.SetActive(true);
        }

        if(Input.GetKey(KeyCode.O))
        {
            retry();
        }


        if(Input.GetKey(KeyCode.Escape))
        {
            pause.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }

    }
    public void home()
    {
        
        SceneManager.LoadScene(0);

    }
    public void retry()
    {
        SceneManager.LoadScene(current);
    }

    public void Continue()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void next()
    {
        SceneManager.LoadScene(current + 1);
        
    }
    public void Settings()
    {
        settings.SetActive(true);
        pause.SetActive(false);
    }
    public void Back()
    {
        settings.SetActive(false);
        pause.SetActive(true);
    }
}
