using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Image background;
    public Slider musicSlider, fxSlider;
    private bool IsShowing => background.gameObject.activeSelf;
    public AudioMixer audioMixer;

    private void Start()
    {
        musicSlider.value = .75f;
        fxSlider.value = .75f;

        if (IsShowing) ToggleUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleUI();
        }

        if (IsShowing)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
            audioMixer.SetFloat("FXVolume", Mathf.Log10(fxSlider.value) * 20);
        }
    }

    public void ToggleUI()
    {
        background.gameObject.SetActive(!IsShowing);
    }

    public void QuitGame()
    {
        //Application.Quit();
    }
}
