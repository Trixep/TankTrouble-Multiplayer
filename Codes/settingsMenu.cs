using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingsMenu : MonoBehaviour
{
    public AudioMixer musicAudio;
    public AudioMixer sfxAudio;
    public TMPro.TMP_Dropdown resDropdown;
    public TMPro.TMP_Text toggleText;
    public Toggle fullscreen;
    public Slider musicSlider;
    public Slider sfxSlider;
    private float music;
    private float sfx;

    Resolution[] resolutions;

    private void Awake()
    {
        int fullscreenState = PlayerPrefs.GetInt("fullscreenState");
        if(fullscreenState == 0)
        {
            fullscreen.isOn = false;
            valueChanged(fullscreen);
        }
        else
        {
            fullscreen.isOn = true;
            valueChanged(fullscreen);
        }

        music = PlayerPrefs.GetFloat("Music");
        sfx = PlayerPrefs.GetFloat("SFX");
        musicSlider.value = music;
        sfxSlider.value = sfx;
    }

    private void Start()
    {
        musicAudio.SetFloat("Music", music);
        sfxAudio.SetFloat("SFX", sfx);
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentRes = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " (" + resolutions[i].refreshRate + ")";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentRes;
        resDropdown.RefreshShownValue();
    }

    public void setResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void setMusic(float volume)
    {
        musicAudio.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    public void setSFX(float volume)
    {
        sfxAudio.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFX", volume);
        PlayerPrefs.Save();
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if(isFullscreen == true)
        {
            PlayerPrefs.SetInt("fullscreenState", 1);
            PlayerPrefs.Save();
            toggleText.text = "On";
        }
        else
        {
            PlayerPrefs.SetInt("fullscreenState", 0);
            PlayerPrefs.Save();
            toggleText.text = "Off";
        }
    }

    public void valueChanged(Toggle t)
    {
        if (t.isOn)
        {
            toggleText.text = "On";
        }
        else
        {
            toggleText.text = "Off";
        }
    }
}
