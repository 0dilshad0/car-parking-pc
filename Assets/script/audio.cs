using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audio : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SfxSlider;

    public void musicControl()
    {
        float value = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(value)*20);
    }  public void SfxControl()
    {
        float value = SfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
    }
}
