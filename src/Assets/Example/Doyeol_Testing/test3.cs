
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class test3 : UdonSharpBehaviour
{


    public GameObject target;
    Animator targetAnimation;
    private bool anime1Flag = false;


    void Start()
    {
        targetAnimation = target.GetComponent<Animator>();
        // anime1Flag = targetAnimation.GetBool("anime1Flag");
    }

    public override void Interact()
    {
        anime1Flag = !anime1Flag;
        targetAnimation.SetBool("anime1Flag", anime1Flag);
        // SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "changeAnimationFlag");
    }

    public void changeAnimationFlag()
    {
        anime1Flag = !anime1Flag;
        targetAnimation.SetBool("anime1Flag", anime1Flag);
    }

}
