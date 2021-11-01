
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BoardScript : UdonSharpBehaviour
{

    bool whiteStartFlag = false;
    bool blackStartFlag = false;

    bool gameFlag = false;
    bool winnerFlag = false;


    // 0: no stone, 1: black, 2: white
    int[,] gameStatus = new int[4,4];
    GameObject[] children;


    void Start()
    {
        children = gameObject.GetComponentsInChildren<GameObject>();
    }

    void initializeGame()
    {
        for(int i=0; i<4; i++)
            for(int j=0; j<4; j++)
                gameStatus[i,j] = 0;

        
        whiteStartFlag = false;
        blackStartFlag = false;
        gameFlag = false;
        winnerFlag = false;
    }

    int[] availableSpots(bool playerFlag) // 0: black, 1: white
    {
        int[] retval = new int[17];
        retval[0] = 0; // available spots count

        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                if(isAvailable(i,j, playerFlag))
                {
                    retval[0]++;
                    retval[retval[0]] = 8*i+j;
                }
            }
        }
        return retval;
    }

    bool isAvailable(int posx, int posy, bool playerFlag)
    {
        if(gameStatus[posx,posy] != 0) return false;
        // North
        int x = posx, y = posy;
        if(++y > 8 || gameStatus[x,y] == 1+boolToInt(playerFlag)) return false;
        // North East
        // East
        // SouthEast
        // South
        // SouthWest
        // West
        // NorthWest
        return false;
    }

    int boolToInt(bool input)
    {
        if(input) return 1;
        return 0;
    }
}
