using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class home : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject modePage;
    public GameObject levelPage;
    public GameObject garagePage;
    public GameObject settingsPage;
    public GameObject morePage;
    public GameObject Lock;
    public GameObject backArrow;
    public GameObject[] cars;

    public GameObject loading;
    public Slider slider;
    
    private int current;

    public Button[] buttons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i=0;i<buttons.Length;i++)
        {
            buttons[i].interactable = false;
        }
        for(int i=0;i<unlockedLevel;i++)
        {
            buttons[i].interactable = true;
        }    
    }
    void Start()
    {
        mainPage.SetActive(true);
        modePage.SetActive(false);
        levelPage.SetActive(false);
        garagePage.SetActive(false);
        settingsPage.SetActive(false);
        morePage.SetActive(false);
        loading.SetActive(false);
        Time.timeScale = 1f;
        current = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current == 0)
        {
            Lock.SetActive(false);
            backArrow.SetActive(true);
        }
        else
        {
            Lock.SetActive(true);
            backArrow.SetActive(false);
        }
        
    }
    public void next()
    {
        mainPage.SetActive(false);
        modePage.SetActive(true);
        levelPage.SetActive(false);
        garagePage.SetActive(false);
        settingsPage.SetActive(false);
        morePage.SetActive(false);
    }
    public void level()
    {
        mainPage.SetActive(false);
        modePage.SetActive(false);
        levelPage.SetActive(true);
        garagePage.SetActive(false);
        settingsPage.SetActive(false);
        morePage.SetActive(false);
    }
    public void garage()
    {
        mainPage.SetActive(false);
        modePage.SetActive(false);
        levelPage.SetActive(false);
        garagePage.SetActive(true);
        settingsPage.SetActive(false);
        morePage.SetActive(false);
    }
    public void settings()
    {
        mainPage.SetActive(true);
        modePage.SetActive(false);
        levelPage.SetActive(false);
        garagePage.SetActive(false);
        settingsPage.SetActive(true);
        morePage.SetActive(false);
    }
    public void more()
    {
        mainPage.SetActive(true);
        modePage.SetActive(false);
        levelPage.SetActive(false);
        garagePage.SetActive(false);
        settingsPage.SetActive(false);
        morePage.SetActive(true);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void homee()
    {
        mainPage.SetActive(true);
        modePage.SetActive(false);
        levelPage.SetActive(false);
        garagePage.SetActive(false);
        settingsPage.SetActive(false);
        morePage.SetActive(false);
    }

    public void NextCar()
    {
        foreach(GameObject i in cars)
        {
            i.SetActive(false);
        }
        current++;   
        if(current==cars.Length)
        {
            current = 0;
        }
        cars[current].SetActive(true);
    }
    public void privCar()
    {
        foreach (GameObject i in cars)
        {
            i.SetActive(false);
        }
        current--;
        if (current == -1)
        {
            current = cars.Length-1;
        }
        cars[current].SetActive(true);
    }
    public void level1(int levelIndex)
    {
        loading.SetActive(true);
        StartCoroutine(load(levelIndex));
    }
    IEnumerator load(int levelIndex)
    {
        AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(levelIndex);
        while (!LoadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(LoadOperation.progress / 0.9f);
            slider.value = progressValue;
            yield return null;
        }
    }
}
 