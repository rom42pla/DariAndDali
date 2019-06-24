using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    public GameObject soundOn;
    public GameObject soundOff;
    public void Exit()
    {
        Application.Quit();
        print("uscito");
    }


    public void ToggleSound()
    {

        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);

        }
        SetSoundState();
    }

    private void SetSoundState()
    {

        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            AudioListener.volume = 1;
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            print("volume: " + AudioListener.volume);
            print(PlayerPrefs.GetInt("Muted", 0));
        }
        else
        {
            AudioListener.volume = 0;
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            print("volume: " + AudioListener.volume);
            print(PlayerPrefs.GetInt("Muted", 0));
        }

    }

    public void Pause(GameObject PauseUI)
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;   
    }

    public void Resume() {
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Reverse(GameObject ui) {
        Vector3 coord = new Vector3(0, 180, 180);
        ui.transform.Rotate(coord);
    }
}
