using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Direction { Left, Right };
    [Header("Player settings")]
    [HideInInspector] public int playerNo;
    [HideInInspector] public int respawnTime = 1;
    // Variabili spaziali
    private Vector3 directionVector;
    [HideInInspector] public float maxSpeed = 5f, jumpForce = 5f;
    [HideInInspector] public KeyCode leftMovementKey = KeyCode.LeftArrow, rightMovementKey = KeyCode.RightArrow, jumpKey = KeyCode.Space;
    private UIButtonController leftMovementBtn, rightMovementBtn, jumpBtn;
    [HideInInspector] public Vector3 lastCheckpoint;
    public GameObject ground = null;
    public float jumpTimeThreshold = 0.05f;
    // Variabili di gravità
    [HideInInspector] public float gravity = 9.81f;
    [HideInInspector] public bool reversedGravity = false;

    public CharacterController controller;
    public SpriteRenderer spriteRenderer;
    private Animator animator;

    private float xSize, ySize;
    private float xSpeed, ySpeed;
    [HideInInspector] public float zPos = 0.5f;

    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip jumpSFX, deathSFX;

    [Header("Infos")]
    [HideInInspector] public bool canMove = true, isMoving = false, isJumping = true, isDying = false, isJoying = false;
    [HideInInspector] public Direction direction = Direction.Right;

    void Start()
    {
        this.controller = this.gameObject.GetComponent<CharacterController>();
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.animator = this.gameObject.GetComponent<Animator>();
        // Alcune caratteristiche peculiari del secondo giocatore
        if(this.playerNo == 2)  {
            this.zPos = -zPos;
            this.direction = Direction.Left;
        }
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.zPos);
        // Le dimensioni del personaggio
        this.xSize = this.controller.bounds.extents.x * 2;
        this.ySize = this.controller.bounds.extents.y * 2;
        // Trova le componenti della UI associate ai tasti
        leftMovementBtn = GameObject.Find("P" + this.playerNo + "LeftBtn").GetComponent<UIButtonController>();
        rightMovementBtn = GameObject.Find("P" + this.playerNo + "RightBtn").GetComponent<UIButtonController>();
        jumpBtn = GameObject.Find("P" + this.playerNo + "JumpBtn").GetComponent<UIButtonController>();
        // Setta le impostazioni audio
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if(this.animator != null){
            this.refreshAnimator();
        }
        //remainOnBinary();
        checkGround();
        if(canMove){
            move();
        }
    }

    void refreshAnimator(){
        this.animator.SetBool("isMoving", this.isMoving);
        this.animator.SetBool("isDying", this.isDying);
        this.animator.SetBool("isJumping", this.isJumping);
        this.animator.SetBool("isJoying", this.isJoying);
    }

    /* Si occupa del movimento del personaggio */
    void move(){
        Vector3 direction = Vector3.zero;
        /* Gestisce il movimento laterale del personaggio */
        if(((leftMovementBtn.isPressed || Input.GetKey(leftMovementKey)) && !reversedGravity) || ((rightMovementBtn.isPressed || Input.GetKey(leftMovementKey)) && reversedGravity)){
            this.isMoving = true;
            if(!this.reversedGravity){
                this.spriteRenderer.flipX = true;
                if(this.direction != Direction.Left)    this.direction = Direction.Left;
            } else {
                this.spriteRenderer.flipX = true;
                if(this.direction != Direction.Right)    this.direction = Direction.Right;
            }            
            direction.x = -maxSpeed;
        }
        else if(((rightMovementBtn.isPressed || Input.GetKey(rightMovementKey)) && !reversedGravity) || ((leftMovementBtn.isPressed || Input.GetKey(rightMovementKey))&& reversedGravity)){
            this.isMoving = true;
            if(!this.reversedGravity){
                this.spriteRenderer.flipX = false;
                if(this.direction != Direction.Left)    this.direction = Direction.Left;
            } else {
                this.spriteRenderer.flipX = false;
                if(this.direction != Direction.Right)    this.direction = Direction.Right;
            }
            direction.x = maxSpeed;    
        }
        else{
            this.isMoving = false;
        }
        /* Gestisce il salto del personaggio */
        if (!this.isJumping){
            ySpeed = 0;
            if (jumpBtn.isPressed || Input.GetKey(jumpKey)){
                audioSource.PlayOneShot(this.jumpSFX);
                this.isJumping = true;
                if(!reversedGravity){
                    ySpeed = jumpForce;
                }
                else{
                    ySpeed = -jumpForce;
                }
                
            } 
        }
        /* Gestisce la gravità */
        if(!reversedGravity){
            ySpeed -= gravity * Time.deltaTime;
        }
        else{
            ySpeed -= -gravity * Time.deltaTime;
        }
        direction.y = ySpeed;
        /* Aggiorna la posizione del personaggio */
        controller.Move(direction * Time.deltaTime);
    }

    /* Si occupa della gestione dell'oggetto su cui si trova il personaggio e del parent */
    void checkGround(){
        GameObject groundObject = this.groundObject();
        // Se si trova su qualcosa...
        if(groundObject != null){
            if(this.isJumping){
                this.isJumping = false;
            }
            if(groundObject != this.ground){
                this.ground = groundObject;
                this.transform.SetParent(this.ground.transform, true);
            } 
        }
        // Se non si trova su qualcosa...
        else {     
            this.ground = null;
            this.transform.SetParent(null);
            StartCoroutine(jumpThreshold());
        }
    }

    /* Costringe il personaggio a rimanere sulla stessa Z */
    public void remainOnBinary(){
        if(this.transform.position.z != this.zPos){
            this.controller.enabled = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.zPos);
            this.controller.enabled = true;
        }
    }

    /* Fa rinascere il personaggio in caso di morte */
    public void respawn(){
        audioSource.PlayOneShot(this.deathSFX);
        Coroutine respawn = StartCoroutine(moveObject(new Vector3(lastCheckpoint.x, lastCheckpoint.y, this.zPos), this.maxSpeed * 2));
    }

    /* Restituisce l'oggetto sul quale si trova il personaggio o altrimenti null */
    GameObject groundObject(){
        GameObject[] collidedObjects;
        if(!this.reversedGravity){
            collidedObjects = (from obj in collidedObjectsInBox(0, -this.ySize/2, this.xSize, 0.4f) select obj.gameObject).ToArray();  
        }
        else{
            collidedObjects = (from obj in collidedObjectsInBox(0, this.ySize/2, this.xSize, 0.4f) select obj.gameObject).ToArray();
        }
        // Ritorna il primo oggetto della lista
        if(collidedObjects.Length != 0){
            return collidedObjects[0]; 
        }
        else{
            return null;
        }     
    }

    /* Restituisce tutti i colliders trovati nell'area indicata, ad eccezione di quello del giocatore */
    GameObject[] collidedObjectsInBox(float xOffset, float yOffset, float xSize, float ySize){
        Collider[] collidedColliders = Physics.OverlapBox(transform.position + new Vector3(xOffset, yOffset, 0), new Vector3(xSize/2, ySize/2, 3f));
        collidedColliders = collidedColliders.Where(collider => (collider != this.controller) && (!collider.isTrigger)).ToArray();
        // Ritorna i gameobjects a partire dai colliders
        GameObject[] collidedObjects = new GameObject[collidedColliders.Length];
        for(int i = 0; i < collidedColliders.Length; i++){
            collidedObjects[i] = collidedColliders[i].gameObject;
        }
        return collidedObjects;
    }

    /* Coroutine che sposta gradualmente un oggetto verso una coordinata */
    public IEnumerator moveObject(Vector3 position, float speed){
        // Impedisce al personaggio di muoversi
        this.transform.SetParent(null, true);
        this.controller.enabled = false;
        this.canMove = false;
        this.isDying = true;
        // Sposta gradualmente il personaggio verso l'ultimo checkpoint
        while (Vector3.Distance(this.gameObject.transform.position, position) > 0.1f){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, position, speed * Time.deltaTime);
            yield return 0;
        }
        // Permette al personaggio di muoversi
        this.controller.enabled = true;
        this.canMove = true;
        this.controller.Move(Vector3.zero);
        this.isDying = false;
        this.gameObject.transform.position = position;
    }

    public IEnumerator jumpThreshold(){
        yield return new WaitForSeconds(this.jumpTimeThreshold);
        if(this.ground == null){
            this.isJumping = true;
        }
        
    }
}
