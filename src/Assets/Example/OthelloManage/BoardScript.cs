
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


    // 0: 돌 없음, 1: 검정 돌, 2: 흰색 돌
    int[] gameStatus = new int[16];
    GameObject[] children;


    void Start()
    {
        // error 있는듯
        children = gameObject.GetComponentsInChildren<GameObject>();
    }

    //승패 판별 후 게임 종료.
    void killGame()
    {
        // 승패 판별
        int counter = 0;
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                counter = gameStatus[4*i+j] == 1 ? counter+1 : counter-1;
            }
        }
        gameFlag = false;
        winnerFlag = counter > 0 ? false : true; // false: 검정 플레이어, true: 흰색 플레이어
    }

    void initializeGame()
    {
        whiteStartFlag = false;
        blackStartFlag = false;
        gameFlag = true;
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                gameStatus[4*i+j]=0;
            }
        }
    }



    // 가능한 지점의 인덱스 리스트를 반환
    int[] availableSpots(bool playerFlag) // playerFlag -> false: 검정 플레이어, true: 흰색 플레이어
    {
        int[] retval = new int[17];
        retval[0] = 0; // 첫번째 값은 가능한 지점의 개수

        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                if(isAvailable(i,j, playerFlag))
                {
                    retval[++retval[0]] = 4*i+j;
                }
            }
        }
        return retval;
    }


    // 지점이 돌을 올릴 수 있는지 판별
    bool isAvailable(int posx, int posy, bool playerFlag)
    {
        int x, y;
        // 돌이 있으면 불가능
        if(gameStatus[4*posx+posy] != 0) return false;


        // 8방향으로 검사
        // 북 (+y)
        x = posx;
        y = posy;
        // 바로 다음 돌이 현재의 돌과 다른 색이여야 가능성 있음
        if(++y<4 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(++y<4)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 북동 (+x, +y)
        x = posx;
        y = posy;
        if(++x<4 && ++y<4 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(++x<4 && ++y<4)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 동 (+x)
        x = posx;
        y = posy;
        if(++x<4 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(++x<4)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 남동 (+x, -y)
        x = posx;
        y = posy;
        if(++x<4 && --y>0 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(++x<4 && --y>0)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 남 (-y)
        x = posx;
        y = posy;
        if(--y>0 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(--y>0)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 남서 (-x, -y)
        x = posx;
        y = posy;
        if(--y>0 && --x>0 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(--y>0 && --x>0)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 서 (-x)
        x = posx;
        y = posy;
        if(--x>0 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(--x>0)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        // 북서 (-x, +y)
        x = posx;
        y = posy;
        if(--x>0 && ++y<4 && gameStatus[4*x+y] == boolToInt(!playerFlag))
        {
            while(--x>0 && ++y<4)
            {
                if(gameStatus[4*x+y] == boolToInt(playerFlag)) return true;
            }
        }
        return false;
    }

    
    // 플레이어 flag -> 보드 정보 (int)의 플레이어 값 (false->1, true->2)
    int boolToInt(bool input)
    {
        if(input) return 2;
        return 1;
    }
}
