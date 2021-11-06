
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

    void Start()
    {
        gameBoard = gameObject.transform.parent.gameObject;
        gameBoardBehaviour = gameBoard.GetComponent<BoardScript>();
        
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        rend.sharedMaterial = materials[0];
        DisableInteractive = true;
    }

    public void setStoneTextureGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setStoneTexture");
    }

    public void setStoneTexture()
    {
        int flag = gameBoardBehaviour.bitmasker.getStatus(gameObject.transform.GetSiblingIndex(), gameBoardBehaviour.g1, gameBoardBehaviour.g2);
        rend.sharedMaterial = materials[flag];
        rend.enabled = flag == 0 ? false : true;
    }

    public void setInteractivenessGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setInteractiveness");
    }

    public void setInteractiveness()
    {
        if(Networking.LocalPlayer.playerId == gameBoardBehaviour.currentTurnPlayerId) DisableInteractive = false;
        else DisableInteractive = true;
    }

    public void setInteractivenessFalseGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "setInteractivenessFalse");
    }

    public void setInteractivenessFalse()
    {
        DisableInteractive = true;
    }

    public override void Interact()
    {
        int myIndex = gameObject.transform.GetSiblingIndex();
        gameBoardBehaviour.updateByInteract(myIndex);
    }
}
