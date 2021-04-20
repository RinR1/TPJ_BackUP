using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private bool deadCheck; // 로봇 사망여부 확인
    private bool actionCheck; // 행동 체크

    public Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider capCol;

    private Vector3 destination;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = idleTime;
        actionCheck = true;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!deadCheck)
        {
            Move();
            TimeCheck();
        }
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
        if(walking || running)
        {
            nav.SetDestination(transform.position + destination * 4f);
        }
    }

    private void ResetAction()
    {
        walking = false; actionCheck = true; running = false;
        nav.speed = walkSpeed;
        anim.SetBool("Walk", walking);
        anim.SetBool("Run", running);
        nav.ResetPath();
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.5f, 1f));
        RandomAction();
    }

    private void RandomAction()
    {
        actionCheck = true;

        int _random = Random.Range(0, 2);

        if(_random == 0)
        {
            TryIdle();
        }
        else
        {
            TryWalk();
        }
    }

    public void TryIdle()
    {
        walking = false;
        running = false;
        currentTime = idleTime;
        Debug.Log("대기");
    }

    public void TryRun(Vector3 _targetPos)
    {
        destination = new Vector3(_targetPos.x - transform.position.x, 0f, _targetPos.z - transform.position.z);
        running = true;
        anim.SetBool("Run", running);
        Debug.Log("러쉬");
    }

    private void TryWalk()
    {
        walking = true;
        anim.SetBool("Walk", walking);
        currentTime = walkTime;
        Debug.Log("이동");
    }

    public void RobotDamage(int _dmg)
    {
        if (!deadCheck)
        {
            robotHp -= _dmg;
        
            if(robotHp <= 0)
            {
                RobotDead();
                Debug.Log("사망했습니다");
            }
        }
    }

    private void RobotDead()
    {
        walking = false;
        running = false;
        deadCheck = true;
        nav.ResetPath();
        anim.SetTrigger("Dead");
        Destroy(this.gameObject, 4f);
    }
}
