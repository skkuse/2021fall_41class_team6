
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UserSubmit : UdonSharpBehaviour
{
    [UdonSynced]
    public string username;
    [UdonSynced]
    public string id;
    [UdonSynced]
    public string email;

    public InputField nameField;
    public InputField idField;
    public InputField emailField;
    Animator nameAnimator;
    Animator idAnimator;
    Animator emailAnimator;

    public Database database;

    public void Start()
    {
        nameAnimator = nameField.gameObject.GetComponent<Animator>();
        idAnimator = idField.gameObject.GetComponent<Animator>();
        emailAnimator = emailField.gameObject.GetComponent<Animator>();
    }

    public void onSubmit()
    {
        VRCPlayerApi player = Networking.LocalPlayer;

        Networking.SetOwner(player, gameObject);

        username = nameField.text;
        id = idField.text;
        email = emailField.text;

        int atPos = email.IndexOf('@');
        bool atCond = atPos != -1 && atPos == email.LastIndexOf('@');
        bool dotCond = !email.EndsWith(".") && email.IndexOf('.') != -1;
        long idNum = 0;
        if (!long.TryParse(id, out idNum) || id.Length > 10)
        {
            idAnimator.SetTrigger("incorrect");
            idField.text = "Wrong ID";
        }
        else if (!atCond || !dotCond)
        {
            emailAnimator.SetTrigger("incorrect");
            emailField.text = "Wrong Email";
        }
        else if (database.idx < database.size - 1) //성공 
        {
            idAnimator.SetTrigger("correct");
            nameAnimator.SetTrigger("correct");
            emailAnimator.SetTrigger("correct");

            if (VRCPlayerApi.GetPlayerCount() == 1)
            {
                database.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "onSubmit");
            }
            else 
            {
                RequestSerialization(); //성공 시에 동기화
            }

            idField.text = "";
            nameField.text = "";
            emailField.text = "";
        }
        else
        {
            idAnimator.SetTrigger("incorrect");
            nameAnimator.SetTrigger("incorrect");
            emailAnimator.SetTrigger("incorrect");

            idField.text = "No More Space";
        }
    }

    public override void OnPostSerialization(SerializationResult result)
    {
        if(result.success)
        {
            //동기화 후에 이벤트 호출
            database.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "onSubmit");
        }
    }
}
