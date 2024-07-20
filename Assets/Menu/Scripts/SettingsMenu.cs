using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float volume = PlayerPrefs.GetFloat("masterVolume");
            masterSlider.value = volume;
        }
        
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float volume = PlayerPrefs.GetFloat("musicVolume");
            musicSlider.value = volume;
        }
        
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            float volume = PlayerPrefs.GetFloat("sfxVolume");
            sfxSlider.value = volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMasterChanged(float value)
    {
        masterSlider.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString("0.00");
        PlayerPrefs.SetFloat("masterVolume", value);
    }
    
    public void OnMusicChanged(float value)
    {
        musicSlider.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString("0.00");
        PlayerPrefs.SetFloat("musicVolume", value);
    }
    
    public void OnSFXChanged(float value)
    {
        sfxSlider.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString("0.00");
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    public void OnBackPress()
    {
        string lastScene = GameManager.instance.lastScene;
        if (lastScene != null)
        {
            SceneManager.LoadScene(lastScene);
        }
    }
}
