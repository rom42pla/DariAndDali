using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World settings")]
    public Vector2 spawnPosition;
    public float xLimit = 128f;
    public float yLimit = 10f;
    [Header("Camera settings")]
    public GameObject cameraPrefab;
    public float distanceFromPlayers = 6f;
    public bool lockedOnZ = false, lockedOnY = true;
    private GameObject camera;
    private CameraController cameraController;
    [Header("Players settings")]
    public GameObject player1Prefab, player2Prefab;
    private GameObject player1, player2;
    private PlayerController player1Controller, player2Controller;
    public bool player1ReversedGravity = false, player2ReversedGravity = true;
    public float maxSpeed = 4.5f, jumpForce = 6.5f, gravity = 9.81f;

    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip levelMusic;
    
    [Header("Background settings")]
    public List<GameObject> backgroundObjects = new List<GameObject>();
    [RangeAttribute(0, 50)] public int maxBackgroundObjects = 30;
    [RangeAttribute(100, 500)] public int minDistance = 100;
    [RangeAttribute(500, 800)] public int maxDistance = 800;
    [RangeAttribute(1, 5)] public int maxRotationSpeed = 3;
    [RangeAttribute(1, 10)] public int maxScalingFactor = 10;

    void Awake()
    {
        // Setta le impostazioni grafiche
        Application.targetFrameRate = -1;
        // Istanzia i giocatori a partire dai loro prefab
        player1 = Instantiate(player1Prefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        player2 = Instantiate(player2Prefab, new Vector3(spawnPosition.x, -spawnPosition.y, 0), Quaternion.Euler(180, 0, 0));
        // Recupera gli script dei controller
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();
        // Setta alcune variabili spaziali dei giocatori
        player1Controller.maxSpeed = this.maxSpeed;
        player2Controller.maxSpeed = this.maxSpeed;
        player1Controller.jumpForce = this.jumpForce;
        player2Controller.jumpForce = this.jumpForce;
        player1Controller.reversedGravity = this.player1ReversedGravity;
        player2Controller.reversedGravity = this.player2ReversedGravity;
        player1Controller.gravity = this.gravity;
        player2Controller.gravity = this.gravity;
        // Setta i checkpoint iniziali dei giocatori
        player1Controller.lastCheckpoint = new Vector3(spawnPosition.x, spawnPosition.y, player1Controller.zPos);
        player2Controller.lastCheckpoint = new Vector3(spawnPosition.x, -spawnPosition.y, player2Controller.zPos);
        // Setta la camera in modo che segua i giocatori
        camera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        cameraController = camera.GetComponent<CameraController>();
        cameraController.player1 = player1;
        cameraController.player2 = player2;
        cameraController.distanceFromPlayers = distanceFromPlayers;
        cameraController.lockedOnZ = this.lockedOnZ;
        cameraController.lockedOnY = this.lockedOnY;
        // Setta le impostazioni audio
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = this.levelMusic;
        audioSource.Play(0);
        // Setta il background
        spawnObjectsInBackground(Random.Range(this.maxBackgroundObjects/2, this.maxBackgroundObjects + 1));
    }

    void Update(){
        // Controlla se i personaggi hanno raggiunto il limite della coordinata Y e, in tal caso, li fa respawnare
        checkYLimit();
    }

    void checkYLimit(){
        if( (!player1Controller.reversedGravity && player1Controller.transform.position.y < -yLimit)
            || (player1Controller.reversedGravity && player1Controller.transform.position.y > yLimit)){
                player1Controller.respawn();
            }
            
        if( (!player2Controller.reversedGravity && player2Controller.transform.position.y < -yLimit)
            || (player2Controller.reversedGravity && player2Controller.transform.position.y > yLimit)){
                player2Controller.respawn();
            }  
    }

    private void spawnObjectsInBackground(int totalObjNumber){
        // Per ogni oggetto di indice objInd...
        for(int objInd = 1; objInd <= totalObjNumber; objInd++){
            // Scegli un oggetto random
            GameObject obj = this.backgroundObjects[Random.Range(0, this.backgroundObjects.Count)];
            IslandController objController = obj.GetComponent<IslandController>();
            // Attribuisci proprietà random
            objController.position = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), Random.Range(this.minDistance, this.maxDistance));
            
            Vector3 rotation = new Vector3(0, 359f, 0);
            if(Random.Range(0f, 1f) >= 0.5f) rotation += new Vector3(359f, 0f, 0f);
            if(Random.Range(0f, 1f) >= 0.5f) rotation += new Vector3(0, -719f, 0f);
            if(Random.Range(0f, 1f) >= 0.5f) rotation += new Vector3(0, 0f, 359f);
            objController.rotation = rotation;
            objController.rotationSpeed = Random.Range(1f, maxRotationSpeed);

            objController.size = new Vector3(Random.Range(1, this.maxScalingFactor), Random.Range(1, this.maxScalingFactor), Random.Range(1, this.maxScalingFactor));
            Instantiate(obj);
        }
    }
}
