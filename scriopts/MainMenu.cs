using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    // This method will be called when the Play button is clicked
    public void PlayGame()
    {
        // Assuming the next scene is index 1 in Build Settings
        SceneManager.LoadScene(1);
    }

    // This method will be called when the Options button is clicked
    public void OpenOptions()
    {
        // Implement your options logic here
        SceneManager.LoadScene(2);
    }

    // This method will be called when the Exit button is clicked
    
}

