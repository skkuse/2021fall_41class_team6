
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class StartButtonScript : UdonSharpBehaviour
{
    GameObject gameBoard;
    BoardScript gameBoardBehaviour;

    void Start()
    {
        Debug.Log("[DORIKA_OUTPUT] Start Button Script initialize");
        gameBoard = gameObject.transform.parent.gameObject;
        gameBoardBehaviour = gameBoard.GetComponent<BoardScript>();
    }

    public override void Interact()
    {
        gameBoardBehaviour.setByStartButton(Networking.LocalPlayer.playerId);
        setInactiveGlobal();
    }

    public void setInactive()
    {
        gameObject.SetActive(false);
    }

    public void setActive()
    {
        gameObject.SetActive(true);
    }

    public void setActiveGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setActive");
    }

    public void setInactiveGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setInactive");
    }
}
