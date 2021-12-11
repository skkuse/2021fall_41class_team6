
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class BoardScript : UdonSharpBehaviour
{
    [UdonSynced]
    public bool whiteStartFlag = false;
    [UdonSynced]
    public bool blackStartFlag = false;
    [UdonSynced]
    public bool currentTurnFlag = false; // false: black, True: white
    [UdonSynced]
    public int currentTurnPlayerId = -1;

    // g1, g2 -> game board bitmask. 0: 돌 없음, 1: 검정 돌, 2: 흰색 돌
    [UdonSynced]
    public ulong g1 = 0;
    [UdonSynced]
    public ulong g2 = 0;

    [UdonSynced]
    public int blackId = -1, whiteId = -1;

    [UdonSynced]
    public int syncFlag = 0;

    void Start()
    {
    }

    public void setByStartButton(int inputId)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        if(!blackStartFlag)
        {
            blackStartFlag = true;
            blackId = inputId;
            syncFlag = 0;
            RequestSerialization();
        }
        else if(!whiteStartFlag)
        {
            whiteStartFlag = true;
            whiteId = inputId;
            syncFlag = 1;
            RequestSerialization();
            // initializeGame();
        }
    }

    public void initializeGame()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        whiteStartFlag = false;
        blackStartFlag = false;
        currentTurnFlag = false;
        g1=g2=0;

        // set middle 4 blocks
        setStatus(27,  1);
        setStatus(28,  2);
        setStatus(35,  2);
        setStatus(36,  1);


        syncFlag = 2;
        RequestSerialization();
        // setTurn();
    }

    public void setTurn()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        currentTurnPlayerId = currentTurnFlag ? whiteId : blackId;
        syncFlag = 3;
        RequestSerialization();
        // int[] spotsInfo = availableSpots();
        // int counter = 0;
        // for(int i=0; i<64; i++)
        // {
        //     gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setStoneTextureGlobal();
        //     if(counter < spotsInfo[0] && i == spotsInfo[counter+1])
        //     {
        //         gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessGlobal();
        //         counter++;
        //     }
        //     else gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessFalseGlobal();
        // }
    }

    public void updateByInteract(int index)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        setStatus(index,  boolToInt(currentTurnFlag));

        int posx = index/8;
        int posy = index%8;
        int[] changingIndex = new int[65];


        //놓은 돌 기준 업데이트
        int x = posx;
        int y = posy;
        // Debug.Log("[OUTPUT] Clicked index = " + index);

        // 북 (+y)
        if(++y<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(++y<8)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(--y > posy)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 북동 (+x, +y)
        x = posx;
        y = posy;
        if(++y<8 && ++x<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(++y<8 && ++x<8)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(--y>posy && --x>posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 동 (+x)
        x = posx;
        y = posy;
        if(++x<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(++x<8)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(--x>posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 남동 (+x, -y)
        x = posx;
        y = posy;
        if(++x<8 && --y>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(++x<8 && --y>=0)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(++y<posy && --x>posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 남 (-y)
        x = posx;
        y = posy;
        if(--y>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(--y>=0)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(++y<posy)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 남서 (-x, -y)
        x = posx;
        y = posy;
        if(--y>=0 &&--x>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(--y>=0 &&--x>=0)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(++y<posy && ++x<posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 서 (-x)
        x = posx;
        y = posy;
        if(--x>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(--x>=0)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(++x<posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        // 북서 (-x, +y)
        x = posx;
        y = posy;
        if(--x>=0 && ++y<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
        {
            while(--x>=0 && ++y<8)
            {
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag))
                {
                    while(++x<posx && --y>posy)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 8*x+y;
                    }
                    break;
                }
            }
        }
        for(int i=0; i<changingIndex[0]; i++)
            setStatus(changingIndex[i+1],  boolToInt(currentTurnFlag));
        // Debug.Log("[OUTPUT] changing list size "+ changingIndex[0]);
        currentTurnFlag = !currentTurnFlag;
        syncFlag = 2;
        RequestSerialization();
        // setTurn();
    }

    
    // 가능한 지점의 인덱스 리스트를 반환
    public int[] availableSpots() // playerFlag -> false: 검정 플레이어, true: 흰색 플레이어
    {
        int[] retval = new int[17];
        retval[0] = 0; // 첫번째 값은 가능한 지점의 개수

        for(int i=0; i<8; i++)
            for(int j=0; j<8; j++)
                if(isAvailable(i,j))
                    retval[++retval[0]] = 8*i+j;
        return retval;
    }


    // 지점이 돌을 올릴 수 있는지 판별
    public bool isAvailable(int posx, int posy)
    {
        int x, y;
        // 돌이 있으면 불가능
        if(getStatus(8*posx+posy) != 0) return false;
        


        // 8방향으로 검사
        // 북 (+y)
        x = posx;
        y = posy;
        // 바로 다음 돌이 현재의 돌과 다른 색이여야 가능성 있음
        if(++y<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(++y<8)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 북동 (+x, +y)
        x = posx;
        y = posy;
        if(++x<8 && ++y<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(++x<8 && ++y<8)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 동 (+x)
        x = posx;
        y = posy;
        if(++x<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(++x<8)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 남동 (+x, -y)
        x = posx;
        y = posy;
        if(++x<8 && --y>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(++x<8 && --y>=0)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 남 (-y)
        x = posx;
        y = posy;
        if(--y>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(--y>=0)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 남서 (-x, -y)
        x = posx;
        y = posy;
        if(--y>=0 && --x>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(--y>=0 && --x>=0)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 서 (-x)
        x = posx;
        y = posy;
        if(--x>=0 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(--x>=0)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        // 북서 (-x, +y)
        x = posx;
        y = posy;
        if(--x>=0 && ++y<8 && getStatus(8*x+y) == boolToInt(!currentTurnFlag))
            while(--x>=0 && ++y<8)
                if(getStatus(8*x+y) == boolToInt(currentTurnFlag)) return true;
        return false;
    }

    //승패 판별 후 게임 종료.
    public void killGame()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        g1=g2=0;
        currentTurnFlag = false;
        currentTurnPlayerId = -1;
        blackId = -1;
        whiteId = -1;
        syncFlag = 4;
        RequestSerialization();
        // for(int i=0; i<64; i++)
        // {
        //     gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setStoneTextureGlobal();
        //     gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessFalseGlobal();
        // }
        // gameObject.transform.GetChild(64).GetComponent<StartButtonScript>().setActiveGlobal();
        // gameObject.transform.GetChild(65).GetComponent<StartButtonScript>().setActiveGlobal();  
    }

    
    // 플레이어 flag -> 보드 정보 (int)의 플레이어 값 (false->1, true->2)
    public int boolToInt(bool input)
    {
        if(input) return 2;
        return 1;
    }

    public int getStatus(int index)
    {
        int outer = index/32;
        int inner = index%32;

        ulong retval = outer == 0 ? g1 : g2; //statusField[outer];
        retval = retval << (inner*2);
        retval = retval >> 62;

        return (int)retval;
    }

    public void setStatus(int index, int targetData)
    {
        int outer = index/32;
        int inner = index%32;

        // statusField[outer] init
        ulong initializer = ~((ulong)3 << ((31-inner)*2));

        if(outer == 0)
        {
            g1 = g1 & initializer;
            g1 = g1 | ((ulong)targetData << ((31-inner)*2));
        }
        else
        {
            g2 = g2 & initializer;
            g2 = g2 | ((ulong)targetData << ((31-inner)*2));
        }
    }

    public override void OnDeserialization()
    {
        switch (syncFlag)
        {
            case 1:
            initializeGame();
            break;
            
            case 2:
            setTurn();
            break;

            case 3:
            int[] spotsInfo = availableSpots();
            int counter = 0;
            for(int i=0; i<64; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setStoneTextureGlobal();
                if(counter < spotsInfo[0] && i == spotsInfo[counter+1])
                {
                    gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessGlobal();
                    counter++;
                }
                else gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessFalseGlobal();
            }
            break;

            case 4:
            for(int i=0; i<64; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setStoneTextureGlobal();
                gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessFalseGlobal();
            }
            gameObject.transform.GetChild(64).GetComponent<StartButtonScript>().setActiveGlobal();
            gameObject.transform.GetChild(65).GetComponent<StartButtonScript>().setActiveGlobal();       
            break;
        }
    }
}
