using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip checkpointSFX;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other){
        // Se il checkpoint incrocia un giocatore (QUALSIASI)...
        if(other.gameObject.tag == "Player" && (other.gameObject.GetComponent<PlayerController>().lastCheckpoint.x != this.gameObject.transform.position.x && other.gameObject.GetComponent<PlayerController>().lastCheckpoint.y != this.gameObject.transform.position.y)){
            // Riproduce il SFX
            audioSource.PlayOneShot(checkpointSFX);
            // Ferma il sistema particellare
            this.gameObject.GetComponent<ParticleSystem>().Stop();
            GameObject player = other.gameObject;
            PlayerController playerController = player.GetComponent<PlayerController>();
            // Setta la posizione dell'ultimo checkpoint a quella di questo checkpoint
            playerController.lastCheckpoint = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        }
    }
}
