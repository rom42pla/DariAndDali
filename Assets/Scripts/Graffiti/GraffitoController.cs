using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitoController : MonoBehaviour
{
    [RangeAttribute(0, 2)] public int playerNo = 1;
    public bool defadeOnPlayerPass = true;
    private GameObject playerTouching = null;
    [HideInInspector] public bool visible = false, isAnimating = false;
    private Coroutine opacityCoroutine = null;
    [RangeAttribute(0f, 1f)] public float fadedOpacity = 0.3f;
    [RangeAttribute(0f, 1f)] public float speed = 0.05f;
    SpriteRenderer renderer;
    [Header("Animation settings")]
    [RangeAttribute(0f, 5f)] public float animationDelay = 1f;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    void Start()
    {
        this.renderer = this.GetComponent<SpriteRenderer>();
        this.renderer.material.color = new Color(this.renderer.color.r, this.renderer.color.g, this.renderer.color.b, fadedOpacity);    
        if(this.sprites.Count != 0)
            StartCoroutine(this.animate());
    }

    public IEnumerator animate(){
        int frame = 0;
        while(true){
            this.renderer.sprite = this.sprites[frame];
            if(frame == this.sprites.Count - 1){
                frame = 0;
            }
            else{
                frame = frame + 1;
            }
            yield return new WaitForSeconds(this.animationDelay);
        }
    }

    /* Cambia lo stato del graffito da visibile a non e viceversa */
    public void changeStatus(){
        if(!this.visible){
            if(this.isAnimating && this.opacityCoroutine != null) {
                StopCoroutine(opacityCoroutine);
            }
            opacityCoroutine = StartCoroutine(changeOpacity(1f, this.speed));
        }
        else{
            if(this.isAnimating && this.opacityCoroutine != null) {
                StopCoroutine(opacityCoroutine);
            }
            opacityCoroutine = StartCoroutine(changeOpacity(fadedOpacity, this.speed));
        }
    }

    /* Eseguita nel momento in cui un giocatore entra nel trigger */
    void OnTriggerEnter(Collider coll)
    {
        // Se la leva viene toccata da un giocatore...
        if(coll.gameObject.tag == "Player" && defadeOnPlayerPass){
            if(coll.gameObject.GetComponent<PlayerController>().playerNo == this.playerNo || this.playerNo == 0){
                this.playerTouching = coll.gameObject;
                changeStatus();
            }
        } 
    }

    /* Eseguita nel momento in cui un giocatore esce dal trigger */
    void OnTriggerExit(Collider coll)
    {
        // Se la leva viene toccata da un giocatore...
        if(coll.gameObject.tag == "Player" && defadeOnPlayerPass){
            if(coll.gameObject.GetComponent<PlayerController>().playerNo == this.playerNo || this.playerNo == 0){
                this.playerTouching = null;
                changeStatus();
            }
        } 
    }

    /* Coroutine che aggiusta gradualmente l'opacità dello sprite */
    public IEnumerator changeOpacity(float opacity, float speed){
        this.visible = !this.visible;
        this.isAnimating = true;
        if(this.renderer.material.color.a <= opacity){
            for(float i = this.renderer.material.color.a; i <= opacity; i += (speed)){
                this.renderer.material.color = new Color(this.renderer.color.r, this.renderer.color.g, this.renderer.color.b, i);
                yield return new WaitForSeconds(0.05f);
            }
        }
        else{
            for(float i = this.renderer.material.color.a; i >= opacity; i -= (speed)){
                this.renderer.material.color = new Color(this.renderer.color.r, this.renderer.color.g, this.renderer.color.b, i);
                yield return new WaitForSeconds(0.05f);
            }
        }
        this.isAnimating = false;
    }
}
