
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class BoardBlockScript : UdonSharpBehaviour
{
    public Material[] materials;
    Renderer rend;
    GameObject gameBoard;
    BoardScript gameBoardBehaviour;
    [UdonSynced]
    int boardFlag = 0;

    void Start()
    {
        gameBoard = gameObject.transform.parent.gameObject;
        gameBoardBehaviour = gameBoard.GetComponent<BoardScript>();
        
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];
        DisableInteractive = true;
    }

    public void setStone(int flag)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        boardFlag = flag;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setStoneTextureGlobal");
    }

    public void setStoneTextureGlobal()
    {
        if(boardFlag==0) rend.sharedMaterial = materials[0];
        else if(boardFlag==1) rend.sharedMaterial = materials[1];
        else if(boardFlag==2) rend.sharedMaterial = materials[2];       
    }

    public void setInteractiveness()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setInteractivenessGlobal");
    }

    public void setInteractivenessGlobal()
    {
        if(Networking.LocalPlayer.playerId == gameBoardBehaviour.currentTurnPlayerId) DisableInteractive = false;
        else DisableInteractive = true;
    }

    public void setInteractivenessFalse()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setInteractivenessFalseGlobal");
    }

    public void setInteractivenessFalseGlobal()
    {
        DisableInteractive = true;
    }

    public override void Interact()
    {
        int myIndex = gameObject.transform.GetSiblingIndex();
        gameBoardBehaviour.updateByInteract(myIndex);
    }
}
