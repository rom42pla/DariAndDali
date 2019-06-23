using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBuilder : MonoBehaviour
{
    

    public void ChangeScene(string name) {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }



}
