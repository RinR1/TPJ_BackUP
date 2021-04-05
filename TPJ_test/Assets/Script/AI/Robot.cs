using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private string robotName; // 로봇 이름
    [SerializeField] private int robotHp; // 로봇 체력

    [SerializeField] private float walkSpeed; // 로봇 이동속도
    [SerializeField] private float runSpeed; // 로봇 달리기속도

    [SerializeField] private float walkTime; // 이동 시간
    [SerializeField] private float idleTime; // 대기 시간

    private float currentTime;

    private bool walking; // 걷기 체크
    private bool running; // 달리기 체크
    private bool actionCheck; // 행동 체크

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider capCol;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = idleTime;
        actionCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Run();
        RandomRotation();
        TimeCheck();
    }

    private void TimeCheck()
    {
        if (actionCheck)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                ResetAction();
        }
    }

    private void Move()
    {
        if (walking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    private void Run()
    {
        if (running)
            rigid.MovePosition(transform.position + (transform.forward * runSpeed * Time.deltaTime));
    }

    private void RandomRotation()
    {
        if (walking || running)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void ResetAction()
    {
        walking = false; actionCheck = true; running = false;
        anim.SetBool("Walk", walking);
        anim.SetBool("Run", running);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        actionCheck = true;
        int _random = Random.Range(0, 3);

        if (_random == 0)
            TryIdle();
        else if(_random == 1)
            TryWalk();
        else
            TryRun();
    }

    private void TryIdle()
    {
        currentTime = idleTime;
        Debug.Log("대기");
    }

    private void TryRun()
    {
        running = true;
        anim.SetBool("Run", running);
        currentTime = walkTime;
        Debug.Log("러쉬");
    }

    private void TryWalk()
    {
        walking = true;
        anim.SetBool("Walk", walking);
        currentTime = walkTime;
        Debug.Log("이동");
    }
}
