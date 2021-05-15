using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FOVRobot : MonoBehaviour
{

    [SerializeField] private float viewAngle; // 시야각
    [SerializeField] private float viewDistance; //시야 거리
    [SerializeField] private LayerMask targetMask; // 시야에 보이는 타겟 (플레이어)


    private Robot r_mine;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        r_mine = GetComponent<Robot>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!r_mine.deadCheck)
        {
            View();
        }
    }
    
    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f); // 0.5f = 1/2 (연산속도 = 곱셈 > 나눗셈)
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask); // OverlapSphere = 주변 콜라이더들을 뽑아 저장

       for(int i= 0; i< _target.Length; i++)
        {
            Transform _targetInfo = _target[i].transform;
            if(_targetInfo.name == "Player")
            {
                Vector3 _direction = (_targetInfo.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if (_angle < viewAngle * 0.5)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            if (Vector3.Distance(transform.position, _hit.transform.position) <= 3.3f)
                            {
                                r_mine.nav.isStopped = true;
                                r_mine.anim.SetBool("Walk", false);
                                r_mine.anim.SetBool("Run", false);
                                r_mine.TryAttack();
                            }
                            else
                            {
                                Debug.Log("타겟 포착");
                                Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                                r_mine.anim.SetBool("Walk", false);
                                r_mine.TryRun(_hit.transform.position);
                            }
                        }
                    }
                }
            }
        }
    }
}
