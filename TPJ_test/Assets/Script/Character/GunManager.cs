using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public string gunName; // 총기 이름
    public float range;    // 사정거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadSpeed; // 재장전 속도

    public int damage;

    public int reloadBulletCount; //총알 재장전 개수
    public int currentBulletCount; //현재 탄장에 남아있는 총알의수
    public int maxBulletCount; // 최대 보유가능한 총알개수
    public int carryBulletCount; // 현재 보유한 총알개수

    public float retroActionForce; // 지향사격시 반동 세기
    public float retroFineSightForce; // 정조준시 반동 세기

    public Vector3 fineSinghtOriginPos; // 정조준시 위치

    public Animator anim;
    public AudioClip fire_sound;
}
