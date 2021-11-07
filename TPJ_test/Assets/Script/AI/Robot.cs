using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    public static bool Quest2ItemActivated = false;

    [SerializeField] private string robotName; // 로봇 이름
    [SerializeField] private int robotHp; // 로봇 체력

    [SerializeField] private float walkSpeed; // 로봇 이동속도
    [SerializeField] private float runSpeed; // 로봇 달리기속도

    [SerializeField] private float walkTime; // 이동 시간
    [SerializeField] private float idleTime; // 대기 시간

    [SerializeField] private float attackDelay; // 로봇 공격 딜레이
    [SerializeField] private float attackDelayA; // 로봇 공격판정 활성화 시점
    [SerializeField] private float attackDelayB; // 로봇 공격판정 비활성화 시점

    [SerializeField] private int attackDamage;

    public Animator anim;
    [SerializeField] PlayerController player;
    [SerializeField] private Status status;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider capCol;
    [SerializeField] private BoxCollider boxCol;

    private float currentTime;
    [SerializeField] private int questItem;

    private bool walking; // 걷기 체크
    private bool running; // 달리기 체크
    public bool deadCheck; // 로봇 사망여부 확인
    private bool attackCheck; // 로봇 공격여부 체크
    private bool actionCheck; // 행동 체크
    private bool damageCheck; // 몬스터 피격체크

    [SerializeField] private GameObject keyPrefab;

    private Vector3 destination;
    public NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = idleTime;
        actionCheck = true;
        nav = GetComponent<NavMeshAgent>();
        questItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!deadCheck)
        {
            Move();
            TimeCheck();
            RobotHitChase();
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
        nav.speed = runSpeed;
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

    public void TryAttack()
    {
        if (!attackCheck)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        attackCheck = true;
        anim.SetTrigger("Close Attack");

        yield return new WaitForSeconds(attackDelayA);
        boxCol.enabled = true; // 히트박스 활성화

        yield return new WaitForSeconds(attackDelayB);
        boxCol.enabled = false; // 히트박스 비활성화

        yield return new WaitForSeconds(attackDelay - attackDelayA - attackDelayB);
        attackCheck = false;
        nav.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.name == "Player")
        {
            Debug.Log("플레이어 피격");
            status.DecreaseHP(attackDamage);
        }
    }

    public void RobotDamage(int _dmg)
    {
        if (!deadCheck)
        {
            robotHp -= _dmg;

            if(robotHp != 100)
            {
                damageCheck = true;
            }

            if(robotHp <= 0)
            {
                RobotDead();
                Debug.Log("사망했습니다");
            }
        }
    }

    private void RobotHitChase()
    {
        if(damageCheck == true)
        {
            actionCheck = false;
            TryRun(player.transform.position);
        }
    }

    private void RobotDead()
    {
        walking = false;
        running = false;
        deadCheck = true;
        nav.ResetPath();
        anim.SetTrigger("Dead");

        if(!Quest2ItemActivated)
        {
            int itemDrop = Random.Range(0, 3);
            if(itemDrop == 1)
            {
                GameObject Key = Instantiate(keyPrefab);
                Key.transform.position = this.transform.position;
                QuestBoxManager.Quest2Clear = true;
            }
        }
        Destroy(this.gameObject, 4f);
    }
}
