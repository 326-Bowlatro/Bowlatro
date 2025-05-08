using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadJokerScene()
    {
        SceneManager.LoadScene("GameScene"); // Make sure this name matches your scene!
    }
}
