
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
public class MoveLightLeft : UdonSharpBehaviour
{
    public GameObject gameObject;
    void Start()
    {

    }

    public override void Interact()
    {

        gameObject.transform.Translate(-0.2f, 0, 0);
    }
}
