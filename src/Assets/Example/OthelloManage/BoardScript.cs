
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


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


    public GameObject bitmaskerObject;
    public BitmaskingScript bitmasker;


    void Start()
    {
        bitmasker = bitmaskerObject.GetComponent<BitmaskingScript>();
    }

    public void setByStartButton(int inputId)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        if(!blackStartFlag)
        {
            blackStartFlag = true;
            // gameObject.transform.GetChild(16).gameObject.SetActive(false);
            Debug.Log("[DORIKA_OUTPUT] black ID : " + blackId + " -> " + inputId);
            blackId = inputId;
            RequestSerialization();
            return;
        }
        if(!whiteStartFlag)
        {
            whiteStartFlag = true;
            // gameObject.transform.GetChild(17).gameObject.SetActive(false);
            Debug.Log("[DORIKA_OUTPUT] white ID : " + whiteId + " -> " + inputId);
            whiteId = inputId;
            RequestSerialization();
            initializeGame();
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
        bitmasker.setStatus(5, g1, g2, 1);
        bitmasker.setStatus(6, g1, g2, 2);
        bitmasker.setStatus(9, g1, g2, 2);
        bitmasker.setStatus(10, g1, g2, 1);
        RequestSerialization();


        // black starts
        setTurn();
    }

    public void setTurn()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        currentTurnPlayerId = currentTurnFlag ? whiteId : blackId;
        RequestSerialization();
        Debug.Log("[DORIKA_OUTPUT] Turn Player ID : " + currentTurnPlayerId);
        int[] spotsInfo = availableSpots();
        int counter = 0;
        for(int i=0; i<16; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setStoneTextureGlobal();
            if(counter < spotsInfo[0] && i == spotsInfo[counter+1])
            {
                gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessGlobal();
                counter++;
            }
            else gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessFalseGlobal();
        }
    }

    public void updateByInteract(int index)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        bitmasker.setStatus(index, g1, g2, boolToInt(currentTurnFlag));

        int posx = index/4;
        int posy = index%4;
        int[] changingIndex = new int[16];


        //놓은 돌 기준 업데이트
        int x = posx;
        int y = posy;
        Debug.Log("[DORIKA_OUTPUT] Clicked index = " + index);

        // 북 (+y)
        if(++y<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(++y<4)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(--y > posy)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 북동 (+x, +y)
        x = posx;
        y = posy;
        if(++y<4 && ++x<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(++y<4 && ++x<4)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(--y>posy && --x>posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 동 (+x)
        x = posx;
        y = posy;
        if(++x<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(++x<4)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(--x>posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 남동 (+x, -y)
        x = posx;
        y = posy;
        if(++x<4 && --y>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(++x<4 && --y>=0)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(++y<posy && --x>posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 남 (-y)
        x = posx;
        y = posy;
        if(--y>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(--y>=0)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(++y<posy)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 남서 (-x, -y)
        x = posx;
        y = posy;
        if(--y>=0 &&--x>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(--y>=0 &&--x>=0)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(++y<posy && ++x<posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 서 (-x)
        x = posx;
        y = posy;
        if(--x>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(--x>=0)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(++x<posx)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        // 북서 (-x, +y)
        x = posx;
        y = posy;
        if(--x>=0 && ++y<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
        {
            while(--x>=0 && ++y<4)
            {
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag))
                {
                    while(++x<posx && --y>posy)
                    {
                        changingIndex[0]++;
                        changingIndex[changingIndex[0]] = 4*x+y;
                    }
                    break;
                }
            }
        }
        for(int i=0; i<changingIndex[0]; i++)
            bitmasker.setStatus(changingIndex[i+1], g1, g2, boolToInt(currentTurnFlag));
        Debug.Log("[DORIKA_OUTPUT] changing list size "+ changingIndex[0]);
        currentTurnFlag = !currentTurnFlag;
        RequestSerialization();
        setTurn();
    }

    
    // 가능한 지점의 인덱스 리스트를 반환
    public int[] availableSpots() // playerFlag -> false: 검정 플레이어, true: 흰색 플레이어
    {
        int[] retval = new int[17];
        retval[0] = 0; // 첫번째 값은 가능한 지점의 개수

        for(int i=0; i<4; i++)
            for(int j=0; j<4; j++)
                if(isAvailable(i,j))
                    retval[++retval[0]] = 4*i+j;
        return retval;
    }


    // 지점이 돌을 올릴 수 있는지 판별
    public bool isAvailable(int posx, int posy)
    {
        int x, y;
        // 돌이 있으면 불가능
        if(bitmasker.getStatus(4*posx+posy, g1,g2) == 0) return false;
        


        // 8방향으로 검사
        // 북 (+y)
        x = posx;
        y = posy;
        // 바로 다음 돌이 현재의 돌과 다른 색이여야 가능성 있음
        if(++y<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(++y<4)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 북동 (+x, +y)
        x = posx;
        y = posy;
        if(++x<4 && ++y<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(++x<4 && ++y<4)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 동 (+x)
        x = posx;
        y = posy;
        if(++x<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(++x<4)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 남동 (+x, -y)
        x = posx;
        y = posy;
        if(++x<4 && --y>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(++x<4 && --y>=0)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 남 (-y)
        x = posx;
        y = posy;
        if(--y>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(--y>=0)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 남서 (-x, -y)
        x = posx;
        y = posy;
        if(--y>=0 && --x>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(--y>=0 && --x>=0)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 서 (-x)
        x = posx;
        y = posy;
        if(--x>=0 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(--x>=0)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        // 북서 (-x, +y)
        x = posx;
        y = posy;
        if(--x>=0 && ++y<4 && bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(!currentTurnFlag))
            while(--x>=0 && ++y<4)
                if(bitmasker.getStatus(4*x+y, g1, g2) == boolToInt(currentTurnFlag)) return true;
        return false;
    }

    //승패 판별 후 게임 종료.
    public void killGame()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        // 승패 판별
        int counter = 0;
        for(int i=0; i<4; i++)
            for(int j=0; j<4; j++)
                counter = bitmasker.getStatus(4*i+j, g1, g2) == 1 ? counter+1 : counter-1;
        g1=g2=0;
        currentTurnFlag = false;
        currentTurnPlayerId = -1;
        blackId = -1;
        whiteId = -1;
        for(int i=0; i<16; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setStoneTextureGlobal();
            gameObject.transform.GetChild(i).GetComponent<BoardBlockScript>().setInteractivenessFalseGlobal();
        }
        gameObject.transform.GetChild(16).GetComponent<StartButtonScript>().setActiveGlobal();
        gameObject.transform.GetChild(17).GetComponent<StartButtonScript>().setActiveGlobal();
    }

    
    // 플레이어 flag -> 보드 정보 (int)의 플레이어 값 (false->1, true->2)
    public int boolToInt(bool input)
    {
        if(input) return 2;
        return 1;
    }
}
