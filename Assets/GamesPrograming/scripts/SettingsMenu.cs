using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer; //mixer obj
    public Dropdown resDropdown; //dropdown UI
    public Resolution[] resolutions; // array for storing the supported resolutions

    void Start()
    {
        resolutions = Screen.resolutions; //get the resolutions

        resDropdown.ClearOptions(); //clear the dropdown options

        List<string> options = new List<string>(); //list to store resolutions
        int currentResIndex = 0;

        for (int i=0; i<resolutions.Length; i++) //for each resolution 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //make res an option
            options.Add(option); //add option to the list 

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(options); //add options to the UI dropdown element
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();

    }

    public void setRes(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen); //set the selected resolution
    }

    public void setVolume(float volume)
    {
        mixer.SetFloat("Volume", volume); //set the exposed parameter of the mixer to the function parameter
        //Debug.Log(volume);
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); //set unity's graphics quality setting 
    }

    public void setFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen; //toggle fullscreen
    }

}
