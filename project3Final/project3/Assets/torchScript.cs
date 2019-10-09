using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class torchScript : MonoBehaviour
{

    [SerializeField] Sprite litSprite;
    private bool lit = false;


    // Update is called once per frame


    public void toggleLight() {
        lit = true;
        GetComponent<SpriteRenderer>().sprite = litSprite;
        GetComponent<Animator>().SetBool("lit", true);
        GetComponentInChildren<Light2D>().intensity = 0.5f;

    }
}
