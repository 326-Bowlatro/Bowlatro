using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadJokerScene()
    {
        SceneManager.LoadScene("JokerModifiers"); // Make sure this name matches your scene!
    }
}
