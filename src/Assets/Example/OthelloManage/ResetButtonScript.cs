
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ResetButtonScript : UdonSharpBehaviour
{
    GameObject gameBoard;
    BoardScript gameBoardBehaviour;

    void Start()
    {
        gameBoard = gameObject.transform.parent.gameObject;
        gameBoardBehaviour = gameBoard.GetComponent<BoardScript>();
    }

    public override void Interact()
    {
        gameBoardBehaviour.killGame();
    }
}
