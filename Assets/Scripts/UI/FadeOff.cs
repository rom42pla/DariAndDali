using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOff : MonoBehaviour
{   
    [SerializeField]
    public int loadScene;
    
    public void OnFadeComplete() {
        print("On fade complete");
        SceneManager.LoadScene(loadScene);
    }

}
