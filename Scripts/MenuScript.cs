using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void goToScene(int scene)
    {
        PlayerPrefs.SetInt("Scene", scene);
        SceneManager.LoadScene("Scenario " + scene);
    }

    public void gotToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void restart()
    {
        int scene = PlayerPrefs.GetInt("Scene");

        //Deleting the data
        File.Delete(Application.persistentDataPath + "/scene" + scene);

        AssetDatabase.Refresh();

        //Restarting the round
        SceneManager.LoadScene("Scenario " + scene);
    }
}
