using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AstroManager : UdonSharpBehaviour
{
    //유저가 버튼을 클릭하면 별자리 사진을 보여주고, 나레이션을 재생합니다.


    // 초기 설정 - 우주 배경이니 중력을 약하게 부여했습니다.
    public VRCPlayerApi playerApi;

    [Header("Player Settings")]
    [SerializeField] float jumpImpulse = 5;
    [SerializeField] float walkSpeed = 4;
    [SerializeField] float runSpeed = 8;
    [SerializeField] float gravityStrengh = 0.2f;

    void Start()
    {
        playerApi = Networking.LocalPlayer;
        playerApi.SetJumpImpulse(jumpImpulse);
        playerApi.SetWalkSpeed(walkSpeed);
        playerApi.SetRunSpeed(runSpeed);
        playerApi.SetGravityStrength(gravityStrengh);
    }

    
    bool lightOn;
    public Light FrameLight; // 별자리 사진을 비추는 빛
    public GameObject Frame; // 별자리 사진
    public GameObject BtnLight; // 버튼을 비추고 있는 빛
    public AudioSource OnSE; // 버튼on 소리
    public AudioSource OffSE; // 버튼off 소리
    public AudioSource Ecliptic; // 나레이션
    public override void Interact()
    {
        if (!Frame.activeSelf) // 별자리 사진 비활성화 중 -> 활성화시키기
        {
            FrameLight.intensity = 0;
            FrameLight.gameObject.SetActive(true);
            lightOn = true;
            OnSE.Play();
            Frame.SetActive(true);
            BtnLight.SetActive(false);
        }
        else // 별자리 사진 활성화 중 -> 비활성화시키기
        {
            FrameLight.gameObject.SetActive(false);
            Ecliptic.Stop();
            OffSE.Play();
            Frame.SetActive(false);
            BtnLight.SetActive(true);
        }
    }

    private void Update()
    {
        if (lightOn) // 빛이 한 번에 확 들어오는 것이 아니라 서서히 밝아지도록 함
        { 
            FrameLight.intensity = Mathf.Lerp(FrameLight.intensity, 1, 0.005f);
            if (FrameLight.intensity > 0.9f)
            {
                Ecliptic.Play();
                lightOn = false;
            }
        }
    }
}
