
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class moveLight : UdonSharpBehaviour
{
    public GameObject light;
    void Start()
    {
          
    }

    public override void Interact()
    {
        light.transform.Translate(0.2f, 0, 0);

    }
}
