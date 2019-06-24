using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [RangeAttribute(0f, 1f)] public float opacity = 0.5f;
    [RangeAttribute(0.1f, 1f)] public float scaleIfPressed = 0.9f;
    private List<GameObject> buttons = new List<GameObject>();
    private List<GameObject> children = new List<GameObject>();

    void Awake()
    {
        // Recupera tutti i componenti della UI
        this.getEveryChildren(this.gameObject);
        this.getButtonChildren(this.gameObject);
        // Regola l'opacità e la grandezza dei componenti
        this.updateOpacity(this.opacity);
        this.updateScaleFactorIfPressed(this.scaleIfPressed);
    }

    void getEveryChildren(GameObject obj){
        foreach(Transform child in obj.transform){
            this.children.Add(child.gameObject);
            getEveryChildren(child.gameObject);
        }
    }

    void getButtonChildren(GameObject obj){
        foreach(GameObject go in this.children){
            if(go.GetComponent<Button>() != null){
                this.buttons.Add(go.gameObject);
            }
        }
    }

    void updateOpacity(float alpha){
        foreach(GameObject UIComponent in this.children){
            Image image = UIComponent.GetComponent<Image>();
            UIImageController imageController = UIComponent.GetComponent<UIImageController>();
            if(image != null){
                if(imageController != null && imageController.faded){
                    image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                }
                else{
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                }
            }
        }
    }

    void updateScaleFactorIfPressed(float scale){
        foreach(GameObject button in this.buttons){
            button.GetComponent<UIButtonController>().scaleFactorIfPressed = scale;
        }
    }
}
