using UdonSharp;
using UnityEngine;
using UnityEngine.EventSystems;
using VRC.SDKBase;
using VRC.Udon;

public class move : UdonSharpBehaviour
{ 
    public GameObject Lights;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "moveLight");
    }

    public void moveLight()
    {
        Lights.transform.Translate(0.2f, 0, 0);
    }


}
