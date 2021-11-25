using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    bool isPressed = false;
    public GameObject Lights;

    // Start is called before the first frame update
    void Start()
    {
        if(isPressed)
        {
            Lights.transform.Translate(0.2f, 0,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
