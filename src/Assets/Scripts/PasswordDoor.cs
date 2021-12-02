
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PasswordDoor : UdonSharpBehaviour
{
    [UdonSynced]
    public string password;

    public Text inputText;
    public Animator inputAnim;
    public GameObject door;
    public GameObject uiObject;

    private void Start()
    {
        password = "thisisnotsolvable";
    }

    public void onSubmit()
    {
        if(inputText.text.Equals(password))
        {
            door.SetActive(false);
            uiObject.SetActive(false);
        }
        else
        {
            inputAnim.SetTrigger("incorrect");
        }
    }

}
