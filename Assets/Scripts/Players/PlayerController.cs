using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNo = 1;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public int respawnTime = 3;
    // Variabili spaziali
    [HideInInspector] public float maxSpeed = 5f, jumpForce = 5f;
    [SerializeField] private KeyCode leftMovementKey = KeyCode.LeftArrow, rightMovementKey = KeyCode.RightArrow;
    private UIButtonController leftMovementBtn, rightMovementBtn, jumpBtn;
    [HideInInspector] public Vector3 lastCheckpoint;
    // Variabili di gravità
    [HideInInspector] public float gravity = 9.81f;
    [HideInInspector] public bool reversedGravity = false;

    private GameObject player;
    private CharacterController controller;

    private float xSize, ySize;
    private float xSpeed, ySpeed;
    [HideInInspector] public float zPos = 0.4f;

    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip jumpSFX;

    void Start()
    {
        this.player = this.gameObject;
        this.controller = this.player.GetComponent<CharacterController>();
        // La posizione del personaggio sull'asse Z, in base al numero del giocatore
        if(this.playerNo == 2)  this.zPos = -0.4f;
        this.player.transform.position = new Vector3(this.player.transform.position.x, this.player.transform.position.y, zPos);
        // Le dimensioni del personaggio
        this.xSize = this.controller.bounds.extents.x;
        this.ySize = this.controller.bounds.extents.y;
        // Trova le componenti della UI associate ai tasti
        leftMovementBtn = GameObject.Find("P" + this.playerNo + "LeftBtn").GetComponent<UIButtonController>();
        rightMovementBtn = GameObject.Find("P" + this.playerNo + "RightBtn").GetComponent<UIButtonController>();
        jumpBtn = GameObject.Find("P" + this.playerNo + "JumpBtn").GetComponent<UIButtonController>();
        // Setta le impostazioni audio
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        remainOnBinary();
        if(canMove){
            move();
        }
    }

    void move(){
        Vector3 direction = Vector3.zero;
        if((leftMovementBtn.isPressed && !reversedGravity) || (rightMovementBtn.isPressed && reversedGravity)
            || (Input.GetKey(leftMovementKey))){
            direction.x = -maxSpeed;
        }
        else if((rightMovementBtn.isPressed && !reversedGravity) || (leftMovementBtn.isPressed && reversedGravity)
            || (Input.GetKey(rightMovementKey))){
            direction.x = maxSpeed;
        }
        if (this.isGrounded()){
            ySpeed = 0;
            if (jumpBtn.isPressed){
                audioSource.PlayOneShot(this.jumpSFX);
                if(!reversedGravity){
                    ySpeed = jumpForce;
                }
                else{
                    ySpeed = -jumpForce;
                }
                
            } 
        }
        if(!reversedGravity){
            ySpeed -= gravity * Time.deltaTime;
        }
        else{
            ySpeed -= -gravity * Time.deltaTime;
        }

        direction.y = ySpeed;
        controller.Move(direction * Time.deltaTime);
    }

    bool isGrounded(){
        if(!reversedGravity) {
            return ((controller.collisionFlags & CollisionFlags.Below) != 0);
        }
        else {
            return ((controller.collisionFlags & CollisionFlags.Above) != 0);
        }
    }
    
    void remainOnBinary(){
        if(this.transform.position.z != 0){
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0f);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit){
        if (hit.gameObject.tag == "Floor"){
            transform.parent = hit.transform;
        }
    }

    GameObject[] isOnPlatform(){
        if(isGrounded()){
            if(!this.reversedGravity){
                GameObject[] collidedPlatforms =
                (from obj in collidedObjectsInBox(0, -this.ySize/2, this.xSize, 0.5f)
                select obj.gameObject).ToArray();
                return collidedPlatforms; 
            }
            else{
                GameObject[] collidedPlatforms =
                (from obj in collidedObjectsInBox(0, this.ySize/2, this.xSize, 0.5f)
                select obj.gameObject).ToArray();
                return collidedPlatforms; 
            }
        }
        else return null;
    }
    
    GameObject[] collidedObjectsInBox(float xOffset, float yOffset, float xSize, float ySize){
        // Ritorna tutti i colliders trovati nell'area, ad eccezione di quello del giocatore
        Collider[] collidedColliders = Physics.OverlapBox(transform.position + new Vector3(xOffset, yOffset, 0), new Vector3(xSize/2, ySize/2, 0.5f));
        collidedColliders = collidedColliders.Where(collider => (collider != this.controller) && (!collider.isTrigger)).ToArray();
        // Ritorna i gameobjects a partire dai colliders
        GameObject[] collidedObjects = new GameObject[collidedColliders.Length];
        for(int i = 0; i < collidedColliders.Length; i++){
            collidedObjects[i] = collidedColliders[i].gameObject;
        }
        return collidedObjects;
    }

    public void respawn(){
        // Respawna il personaggio
        Coroutine respawn = StartCoroutine(moveObject(new Vector3(lastCheckpoint.x, lastCheckpoint.y, this.zPos), this.maxSpeed * 2));
        StartCoroutine(moveTimeout(new Vector3(lastCheckpoint.x, lastCheckpoint.y, this.zPos), respawnTime, respawn));
    }

    /* Coroutine che muove gradualmente un oggetto verso una coordinata */
    public IEnumerator moveObject(Vector3 position, float speed){
        // Annulla il character controller altrimenti non funziona
        controller.enabled = false;
        // Impedisce al personaggio di muoversi
        this.canMove = false;
        while (Vector3.Distance(this.gameObject.transform.position, position) > speed * Time.deltaTime){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, position, speed * Time.deltaTime);
            yield return 0;
        }
        this.gameObject.transform.position = position;
        // Riattiva il character controller
        controller.enabled = true;
        // Permette al personaggio di muoversi
        this.canMove = true;
    }

    public IEnumerator moveTimeout(Vector3 position, int timeout, Coroutine coroutine){
        for (int i = 0; i < timeout; i++){
            yield return new WaitForSeconds(1);
        }
        // Se la coroutine si è buggata
        if(!controller.enabled){
            // Stoppala
            StopCoroutine(coroutine);
            // Riattiva il character controller
            controller.enabled = true;
            // Permette al personaggio di muoversi
            this.canMove = true;
            this.gameObject.transform.position = position;  
        }
    }
   
}
