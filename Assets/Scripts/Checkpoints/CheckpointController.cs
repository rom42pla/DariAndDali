using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [HideInInspector] public bool wasActivated = false;
    [Header("Player settings")]
    [RangeAttribute(1, 2)] public int playerNo = 1;
    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip checkpointSFX;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other){
        // Se il checkpoint incrocia il giocatore giusto...
        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().playerNo == this.playerNo && !this.wasActivated){
            // Riproduce il SFX
            audioSource.PlayOneShot(checkpointSFX);
            // Ferma il sistema particellare
            this.gameObject.GetComponent<ParticleSystem>().Stop();
            this.wasActivated = true;
            GameObject player = other.gameObject;
            PlayerController playerController = player.GetComponent<PlayerController>();
            // Setta la posizione dell'ultimo checkpoint a quella di questo checkpoint
            playerController.lastCheckpoint = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        }
    }
}
