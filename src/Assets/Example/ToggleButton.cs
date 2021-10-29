
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ToggleButton : UdonSharpBehaviour
{
    public GameObject door;
    bool is_open = false;
    Animator doorAnimator;

    public void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "onDoorOpen");
    }

    public void onDoorOpen()
    {
        is_open = !is_open;
        doorAnimator.SetBool("isOpen", is_open);
    }

}
