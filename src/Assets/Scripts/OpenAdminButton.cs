
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class OpenAdminButton : UdonSharpBehaviour
{
    bool open = false;
    public GameObject door;
    public Animator warningAnim;
    public Text btnText;

    public void onClick()
    {
        if(Networking.IsMaster)
        {
            open = !open;
            door.SetActive(!open);
            if(open)
            {
                btnText.text = "close";
            }
            else
            {
                btnText.text = "open";
            }
        }
        else
        {
            warningAnim.SetTrigger("warn");
        }
    }
}
