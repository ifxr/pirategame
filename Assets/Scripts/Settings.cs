using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

//controls settings menu
public class Settings : MonoBehaviour
{
    Resolution[] resolutions;

    private TMP_Dropdown resolutionDropdown;

    [SerializeField]
    public AudioMixer mainAudioMixer;

    private void Awake()
    {
        resolutionDropdown = transform.Find("Dropdown_Resolution").GetComponent<TMP_Dropdown>();
    }

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        //initialize resolution dropdown
        int curResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].ToString(); //obtains resolution option string

            resolutionOptions.Add(option);

            if (Screen.currentResolution.width == resolutions[i].width && Screen.currentResolution.height == resolutions[i].height) //finds current resolution setting
            {
                curResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = curResolutionIndex; //sets currently selected option to be the currrent resolution setting
        resolutionDropdown.RefreshShownValue();
    }

    public void ToggleFullscreen(bool isFullscreen) //called by fullscreen toggle
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) //called by resolution dropdown
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
        Application.targetFrameRate = resolutions[resolutionIndex].refreshRate;
    }

    public void SetVolume(float volume) //sets volume, called by bolume slider
    {
        mainAudioMixer.SetFloat("mainVolume", volume);
    }
}

