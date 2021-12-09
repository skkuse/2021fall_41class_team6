
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PointBtn : UdonSharpBehaviour
{
    //유저가 마우스 클릭을 하는 상태에서 에임 있는 곳에 레이저를 쏴주려고 합니다.
    public AstroManager am;
    bool pointOn = false;
    public GameObject LaserPivot;
    public override void Interact() // 레이저 On/Off
    {
        if (pointOn)
        {
            pointOn = false;
            am.OffSE.Play();
            LaserPivot.SetActive(false);
        }
        else
        {
            pointOn = true;
            am.OnSE.Play();
        }
    }
    private void Update()
    {
        if (pointOn)
        {
            if (Input.GetMouseButton(0)) // 레이저 on 상태, 좌클릭을 하고 있을 때
            {
                if (!LaserPivot.activeSelf) LaserPivot.SetActive(true);
                LaserPivot.transform.rotation = am.playerApi.GetRotation();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                LaserPivot.SetActive(false);
            }
        }
    }
}
