
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void goToScene(int scene)
    {
        PlayerPrefs.SetInt("Scene", scene);
        SceneManager.LoadScene("Scenario " + scene);
    }

    public void gotToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
