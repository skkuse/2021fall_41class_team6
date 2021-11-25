using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    bool isPressed = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed)
        {
            player.transform.Translate(0.2f, 0, 0);
        }
    }
    public void onPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
    public void onPoninterOn(PointerEventData eventData)
    {
        isPressed = false;
    }
}
