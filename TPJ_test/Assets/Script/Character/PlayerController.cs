using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GunManager gm_mine;

    [SerializeField] // private값을 유니티상에서 수정가능하도록 보여줌
    private float walkSpeed; // 캐릭터 걷기속도
    [SerializeField]
    private float runSpeed; // 캐릭터 달리기속도
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed; // 상태 변화에 따른 적용속도

    [SerializeField]
    private float jumpForce; // 점프 높이 조절

    // 상태 체크 변수들
    private bool walkCheck = false; // 이동상태 체크
    private bool runCheck = false; // 달리기상태 체크
    public bool crouchCheck = false; // 앉기상태 체크
    private bool jumpCheck = true; // 지면과의 접촉 체크

    //이동상태 체크용 변수
    private Vector3 lastPos;

    // 앉기,엎드리기 상태변경
    [SerializeField]
    private float crouchPosY;
    private float originPosY;

    private float applyCrouchPosY;

    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 시야각 제한
    [SerializeField]
    private float cameraRotationLimite;

    public float cameraCurrentRotationX = 0f;

    // 필요 컴포넌트
    [SerializeField]
    private Camera c_mine;
    private Rigidbody m_rigid;
    private CapsuleCollider cap_collider;
    private GunController gunController;
    private Crosshair cro_mine;
    private Status st_mine;

    [SerializeField]
    private GunController gu_Mine;

    public bool isActive = true;

    public float testcoil;
    private void Awake()
    {
        if(FindObjectsOfType<PlayerController>().Length != 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cap_collider = GetComponent<CapsuleCollider>();
        m_rigid = GetComponent<Rigidbody>();
        gunController = FindObjectOfType<GunController>();
        cro_mine = FindObjectOfType<Crosshair>();
        st_mine = FindObjectOfType<Status>();

        applySpeed = walkSpeed;
        originPosY = c_mine.transform.localPosition.y;
        applyCrouchPosY = originPosY;

    }

    // Update is called once per frame
    void Update()
    {
        if (Inventory.InventoryActivated != true && ActionController.TextboxActivated != true && MainSceneChanger.PauseActivated != true && MainSceneChanger.GameClearActivated != true && Status.PlayerDead != true)
        {
            if (isActive)
            {
                JumpCheck();
                TryJump();
                TryRun();
                TryCrouch();
                PlayerMove();
                CameraRotation();
                CharacterRotation();
            }
        }
    }

    // 지면 접촉 체크
    private void JumpCheck()
    {
        jumpCheck = Physics.Raycast(transform.position, Vector3.down, cap_collider.bounds.extents.y + 0.3f);
        cro_mine.RunningAnimation(!jumpCheck);
    }

    // 캐릭터 점프 체크
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCheck && st_mine.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    // 캐릭터 점프
    private void Jump()
    {
        if (crouchCheck)
        {
            Crouch();
        }

        st_mine.DecreaseSp(100);
        m_rigid.velocity = transform.up * jumpForce;
    }

    // 캐릭터 달리기 체크
    private void TryRun()
    {
        if (!crouchCheck)
        {
            if (Input.GetKey(KeyCode.LeftShift) && st_mine.GetCurrentSP() > 0)
            {
                Running();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || st_mine.GetCurrentSP() <= 0)
            {
                RunningCancle();
            }
        }
    }

    // 캐릭터 달리기
    private void Running()
    {
        if (crouchCheck)
        {
            Crouch();
        }

        gunController.CancleFineSight();

        runCheck = true;
        cro_mine.RunningAnimation(runCheck);
        st_mine.DecreaseSp(2);
        applySpeed = runSpeed;
    }

    // 캐릭터 달리기 취소
    private void RunningCancle()
    {
        runCheck = false;
        cro_mine.RunningAnimation(runCheck);
        applySpeed = walkSpeed;
    }

    // 캐릭터 앉기 체크
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && jumpCheck)
        {
            Crouch();
        }
    }

    // 캐릭터 앉기
    private void Crouch()
    {
        crouchCheck = !crouchCheck;
        cro_mine.CrouchingAnimation(crouchCheck);

        if (crouchCheck)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(MoveCrouch());
    }

    // 코루틴을 통한 병렬처리(동시에 작동) / 부드러운 캐릭터 앉기
    IEnumerator MoveCrouch()
    {
        float _posY = c_mine.transform.localPosition.y;
        int count = 0; // int를 통한 반복횟수 제어
        while(_posY != applyCrouchPosY) // Lerp를 통해 _posY값을 applyCrouchPos까지 계속 변경
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.1f);
            c_mine.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 30)
            {
                break;
            }
            yield return null;
        }
        c_mine.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    // 캐릭터 이동
    private void PlayerMove()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirZ = Input.GetAxis("Vertical");


        Vector3 _moveHorizontal = transform.right * _moveDirX; // 좌우 이동방향 체크
        Vector3 _moveVertical = transform.forward * _moveDirZ; // 앞뒤 이동방향 체크

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        m_rigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        if (!runCheck && !crouchCheck && jumpCheck)
        {
            if (_velocity != Vector3.zero)
            {
                walkCheck = true;
            }
            else
            {
                walkCheck = false;
            }
            cro_mine.WalkingAnimation(walkCheck);
            lastPos = transform.position;
        }
    }

    // 카메라 상하 회전설정
    private void CameraRotation()
    {
        float mouseRotationX = Input.GetAxisRaw("Mouse Y");
        float c_rotationX = mouseRotationX * lookSensitivity;
        cameraCurrentRotationX -= c_rotationX;
        cameraCurrentRotationX = Mathf.Clamp(cameraCurrentRotationX, -cameraRotationLimite, cameraRotationLimite); // Clamp를 통한 카메라 시야각 제한

        c_mine.transform.localRotation = Quaternion.Euler((cameraCurrentRotationX - gu_Mine.camrecoil < -45 ? -45 : cameraCurrentRotationX - gu_Mine.camrecoil), 0f, 0f);
    }

    // 캐릭터 좌우 회전설정(카메라 좌우)
    private void CharacterRotation()
    {
        float mouseRotationY = Input.GetAxisRaw("Mouse X");
        Vector3 c_RotationY = new Vector3(0f, mouseRotationY, 0f) * lookSensitivity;
        m_rigid.MoveRotation(m_rigid.rotation * Quaternion.Euler(c_RotationY));
    }

}
