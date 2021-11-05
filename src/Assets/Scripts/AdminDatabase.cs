
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class AdminDatabase : UdonSharpBehaviour
{
    public PasswordDoor[] doors;
    public Text descText;
    public Text inputText;
    public Animator inputAnim;
    string[] passwords;
    int idx = 0;

    private void Start()
    {
        passwords = new string[doors.Length];
    }

    public void onSubmit()
    {
        if(Networking.IsMaster)
        {
            string password = inputText.text;
            doors[idx].password = password;
            doors[idx].RequestSerialization();
            passwords[idx] = password;
            inputAnim.SetTrigger("correct");
        }
    }

    public void setNext()
    {
        if(idx <  doors.Length - 1)
        {
            idx++;
        }
        else
        {
            idx = 0;
        }
        descText.text = string.Format("{0}번 부스", idx + 1);
        inputText.text = passwords[idx];
    }
    public void setPrev()
    {
        if (idx > 0)
        {
            idx--;
        }
        else
        {
            idx = doors.Length - 1;
        }
        descText.text = string.Format("{0}번 부스", idx + 1);
        inputText.text = passwords[idx];
    }
}
