using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Level01");
    }
    
    public void OnSettingsClick()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    
    public void OnQuitClick()
    {
        Application.Quit();
    }
}
