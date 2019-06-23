using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    // La posizione della leva
    private Vector3 pos;
    [Header("Status settings")]
    // Variabili di stato
    public bool isActive = false;
    
    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip switchSFX;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        // Setta la posizione iniziale della leva
        this.pos = this.transform.position;
    }

    /* Eseguita nel momento in cui un giocatore entra nel trigger */
    void OnTriggerEnter(Collider coll)
    {
        // Se la leva viene toccata da un giocatore...
        if(coll.gameObject.tag == "Player"){
            Debug.Log("KKK");
            // La leva cambia stato
            changeStatus();
        }
        
    }

    /* Cambia lo stato della leva da attiva a non attiva e viceversa */
    void changeStatus(){
        isActive = !isActive;
        audioSource.PlayOneShot(switchSFX);
        this.transform.Rotate(0, 180, 0, Space.Self);
    }

}
