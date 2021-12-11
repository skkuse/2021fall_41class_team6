
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class MoveLight : UdonSharpBehaviour
{
    public GameObject gameObject;
    void Start()
    {
        
    }

    public override void Interact()
    {

        gameObject.transform.Translate(0.4f, 0, 0);
    }
}
