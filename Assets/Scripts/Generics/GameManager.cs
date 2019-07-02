using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World settings")]
    public Vector2 spawnPosition;
    public float xLimit = 128f;
    public float yLimit = 10f;
    public float gravity = 9.81f;

    [Header("Controls")]
    private KeyCode player1LeftMovementKey = KeyCode.A, player1RightMovementKey = KeyCode.D, player1JumpKey = KeyCode.Space;
    private KeyCode player2LeftMovementKey = KeyCode.LeftArrow, player2RightMovementKey = KeyCode.RightArrow, player2JumpKey = KeyCode.Return;

    [Header("UI settings")]
    public GameObject canvas;
    public GameObject endLevelPortal;
    private bool isFadingOut = false;

    [Header("Camera settings")]
    public GameObject cameraPrefab;
    public float distanceFromPlayers = 6f;
    public bool lockedOnZ = false, lockedOnY = true;
    private GameObject camera;
    private CameraController cameraController;

    [Header("Players settings")]
    public GameObject player1Prefab; 
    public GameObject player2Prefab;
    private GameObject player1, player2;
    private PlayerController player1Controller, player2Controller;
    public bool player1ReversedGravity = false, player2ReversedGravity = true;
    public int respawnTime = 3;
    public float maxSpeed = 4.5f, jumpForce = 6.5f;

    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip levelMusic;
    
    [Header("Background settings")]
    public Color backgroundColor = new Color(255, 255, 255);
    public bool generateBackgroundPlanets = false;
    public List<GameObject> backgroundObjects = new List<GameObject>();
    [RangeAttribute(0, 50)] public int minBackgroundObjects = 2;
    [RangeAttribute(0, 50)] public int maxBackgroundObjects = 6;
    [RangeAttribute(100, 500)] public int minDistance = 100;
    [RangeAttribute(500, 800)] public int maxDistance = 800;
    [RangeAttribute(1, 5)] public int maxRotationSpeed = 3;
    [RangeAttribute(1, 10)] public int maxScalingFactor = 10;

    void Start()
    {
        // Setta le impostazioni grafiche
        Application.targetFrameRate = -1;
        // Istanzia la UI
        Instantiate(canvas);
        // Istanzia i giocatori a partire dai loro prefab
        player1 = Instantiate(player1Prefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        player2 = Instantiate(player2Prefab, new Vector3(spawnPosition.x, -spawnPosition.y, 0), Quaternion.Euler(180, 0, 0));
        // Recupera gli script dei controller
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();
        player1Controller.playerNo = 1;
        player2Controller.playerNo = 2;
        // Setta i comandi dei giocatori
        player1Controller.leftMovementKey = player1LeftMovementKey;
        player1Controller.rightMovementKey = player1RightMovementKey;
        player1Controller.jumpKey = player1JumpKey;
        player2Controller.leftMovementKey = player2LeftMovementKey;
        player2Controller.rightMovementKey = player2RightMovementKey;
        player2Controller.jumpKey = player2JumpKey;
        // Setta alcune variabili spaziali dei giocatori
        player1Controller.respawnTime = this.respawnTime;
        player2Controller.respawnTime = this.respawnTime;
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
        if(generateBackgroundPlanets){
            if(minBackgroundObjects >= maxBackgroundObjects){
                spawnObjectsInBackground(this.maxBackgroundObjects + 1);
            }
            else{
                spawnObjectsInBackground(Random.Range(this.minBackgroundObjects, this.maxBackgroundObjects + 1));
            }
        }
        else{
            this.camera.GetComponent<Camera>().backgroundColor = this.backgroundColor;
        }
    }

    void Update(){
        // Controlla se i personaggi hanno raggiunto il limite della coordinata Y e, in tal caso, li fa respawnare
        checkYLimit();
        if(this.endLevelPortal != null){
            if(this.endLevelPortal.GetComponent<DoubleButtonsDoorsController>().areDoorsOpen && !this.isFadingOut){
                this.player1Controller.isJoying = true;
                this.player2Controller.isJoying = true;
                this.player1Controller.canMove = false;
                this.player2Controller.canMove = false;
                this.player1Controller.controller.enabled = false;
                this.player2Controller.controller.enabled = false;
                StartCoroutine(fade(255, 255, 255));
            }
        }
    }

    void checkYLimit(){
        if( !player1Controller.isDying && ((!player1Controller.reversedGravity && player1Controller.transform.position.y < -yLimit)
            || (player1Controller.reversedGravity && player1Controller.transform.position.y > yLimit))){
                player1Controller.respawn();
            }
            
        if( !player2Controller.isDying && ((!player2Controller.reversedGravity && player2Controller.transform.position.y < -yLimit)
            || (player2Controller.reversedGravity && player2Controller.transform.position.y > yLimit))){
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
            objController.position = new Vector3(Random.Range(-500, 500), Random.Range(-100, 100), Random.Range(this.minDistance, this.maxDistance));
            
            Vector3 rotation = new Vector3(359f, 359f, 359f);
            if(Random.Range(0f, 1f) >= 0.5f) rotation += new Vector3(-719f, 0f, 0f);
            if(Random.Range(0f, 1f) >= 0.5f) rotation += new Vector3(0, -719f, 0f);
            if(Random.Range(0f, 1f) >= 0.5f) rotation += new Vector3(0, 0f, -719f);
            objController.rotation = rotation;
            objController.rotationSpeed = Random.Range(1f, maxRotationSpeed);
            int objectSize = Random.Range(1, this.maxScalingFactor);
            objController.size = new Vector3(objectSize, objectSize, objectSize);
            Instantiate(obj);
        }
    }

    public IEnumerator fade(float r, float g, float b){
        this.isFadingOut = true;
        /* Implementa qui il fade, deve durare esattamente cinque secondi */
        yield return null;
    }
}
