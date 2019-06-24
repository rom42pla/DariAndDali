using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSound : MonoBehaviour
{   [SerializeField]
    private GameObject soundOn;
    [SerializeField]
    private GameObject soundOff;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {   
            if(soundOn != null && soundOff != null) {
                soundOn.SetActive(true);
                soundOff.SetActive(false);
            }
            AudioListener.volume = 1;            
        }
        else
        {
            if (soundOn != null && soundOff != null)
            {
                soundOn.SetActive(false);
                soundOff.SetActive(true);
            }
            AudioListener.volume = 0;
        }
    }
}
