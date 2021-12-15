
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class BoothNameUI : UdonSharpBehaviour
{
    InputField inputField;
    public Text nameText;

    [UdonSynced]
    string boothname;

    void Start()
    {
        inputField = gameObject.GetComponent<InputField>();
    }

    public void onSubmit()
    {
        VRCPlayerApi player = Networking.LocalPlayer;

        Networking.SetOwner(player, gameObject);

        boothname = inputField.text;

        if (VRCPlayerApi.GetPlayerCount()==1)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setName");
        }
        else
        {
            RequestSerialization(); //성공 시에 동기화
        }
    }

    public void setName() {
        nameText.text = boothname;
    }

    public override void OnPostSerialization(SerializationResult result)
    {
        if (result.success)
        {
            //동기화 후에 이벤트 호출
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setName");
        }
    }
}
