using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageController : MonoBehaviour
{
    public bool faded = false;
    [Header("Animation settings")]
    [RangeAttribute(0f, 5f)] public float animationDelay = 1f;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    void Start()
    {  
        if(this.sprites.Count != 0)
            StartCoroutine(this.animate());
    }

    void OnEnable()
    {
        if(this.sprites.Count != 0) 
            StartCoroutine(this.animate());
    }

    public IEnumerator animate(){
        int frame = 0;
        while(true){
            this.GetComponent<Image>().sprite = this.sprites[frame];
            if(frame == this.sprites.Count - 1){
                frame = 0;
            }
            else{
                frame = frame + 1;
            }
            yield return new WaitForSeconds(this.animationDelay);
        }
    }
}
