using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player_mine;

    // 현재 장착된 총
    [SerializeField]
    private GunManager currentGunManager;

    // 연사 속도 계산
    private float currentFireRate;

    // 상태변화 체크
    [SerializeField] private bool reloadCheck = false;
    private bool fineSightCheck = false;

    // 본래 포지션값
    [SerializeField]
    private Vector3 originPos;

    private AudioSource audioSource;

    private RaycastHit hitInfo;
    [SerializeField] LayerMask layerMask;

    [SerializeField]
    private Camera hitCam;

    [SerializeField]
    private Crosshair cro_mine;

    [SerializeField]
    private GameObject enemyHit_Effect; // 적 피격 이펙트
    [SerializeField]
    private GameObject nomalHit_Effect; // 디폴트 피격 이펙트

    public bool isActive = true;
    public bool isRebound = false;

    public float camrecoil;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cro_mine = FindObjectOfType<Crosshair>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Inventory.InventoryActivated != true && ActionController.TextboxActivated != true && MainSceneChanger.PauseActivated != true && MainSceneChanger.GameClearActivated != true && Status.PlayerDead != true)
        {
            if (isActive)
            {
                GunFireRateCal();
                TryFire();
                TryReload();
                TryFineSight();
            }
        }
    }

    // 연사속도 재계산
    private void GunFireRateCal()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    //발사 체크
    private void TryFire()
    {
        if(Time.deltaTime != 0)
        {
            if (Input.GetButton("Fire1") && currentFireRate <= 0 && !reloadCheck)
            {
                RoopFire();
            }
        }

    }

    //발사 전 계산
    private void RoopFire()
    {
        if (!reloadCheck)
        {
            if(currentGunManager.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                CancleFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    //발사 후 계산
    private void Shoot()
    {
        cro_mine.FireAnimation();
        PlaySE(currentGunManager.fire_sound);
        currentGunManager.currentBulletCount--;
        currentFireRate = currentGunManager.fireRate; //연사속도 재계산
        TargetHit();
        StopAllCoroutines();
        StartCoroutine(RetroAction());
    }

    private void TargetHit()
    {
        if (Physics.Raycast(hitCam.transform.position, hitCam.transform.forward + 
            new Vector3(Random.Range(-cro_mine.GunAccuracy() - currentGunManager.accuracy, cro_mine.GunAccuracy() + currentGunManager.accuracy),
                        Random.Range(-cro_mine.GunAccuracy() - currentGunManager.accuracy, cro_mine.GunAccuracy() + currentGunManager.accuracy),
                        0)
            , out hitInfo, currentGunManager.range, layerMask))
        {
            Debug.Log(hitInfo.transform.name);

            if (hitInfo.transform.tag == "Enemy")
            {
                hitInfo.transform.GetComponent<Robot>().RobotDamage(currentGunManager.gunDamage);
                GameObject cloan = Instantiate(enemyHit_Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(cloan, 0.5f);
            }
            else
            {
                GameObject cloan2  = Instantiate(nomalHit_Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(cloan2, 0.5f);
            }
        }
    }

    // 재장전 체크
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !reloadCheck && currentGunManager.currentBulletCount < currentGunManager.reloadBulletCount && currentGunManager.carryBulletCount != 0)
        {
            CancleFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    // 재장전
    IEnumerator ReloadCoroutine()
    {
        PlaySE(currentGunManager.reload_sound);
        if (currentGunManager.carryBulletCount > 0)
        {
            reloadCheck = true;

            yield return new WaitForSeconds(currentGunManager.reloadSpeed);

            if (currentGunManager.carryBulletCount >= currentGunManager.reloadBulletCount)
            {
                currentGunManager.carryBulletCount -= currentGunManager.reloadBulletCount - currentGunManager.currentBulletCount;
                currentGunManager.currentBulletCount = currentGunManager.reloadBulletCount;
            }
            else
            {
                if(currentGunManager.currentBulletCount + currentGunManager.carryBulletCount >= currentGunManager.reloadBulletCount)
                {
                    currentGunManager.carryBulletCount = (currentGunManager.currentBulletCount + currentGunManager.carryBulletCount) - currentGunManager.reloadBulletCount;
                    currentGunManager.currentBulletCount = currentGunManager.reloadBulletCount;
                }
                else
                {
                    currentGunManager.currentBulletCount += currentGunManager.carryBulletCount;
                    currentGunManager.carryBulletCount = 0;
                }

            }

            reloadCheck = false;
        }
        else
        {
            reloadCheck = true;
            Debug.Log("보유한 탄약이 없습니다");
        }
    }

    // 정조준 체크
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !reloadCheck)
        {
            FineSight();
        }
    }

    //정조준 취소
    public void CancleFineSight()
    {
        if (fineSightCheck)
        {
            FineSight();
        }
    }

    //정조준 로직
    private void FineSight()
    {
        fineSightCheck = !fineSightCheck;
        cro_mine.FineSightAnimation(fineSightCheck);

        if (fineSightCheck)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActive());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactive());
        }
    }

    //정조준 활성화
    IEnumerator FineSightActive()
    {
        while(currentGunManager.transform.localPosition != currentGunManager.fineSinghtOriginPos)
        {
            currentGunManager.transform.localPosition = Vector3.Lerp(currentGunManager.transform.localPosition, currentGunManager.fineSinghtOriginPos, 0.2f);
            yield return null;
        }
    }

    //정조눈 비활성화
    IEnumerator FineSightDeactive()
    {
        while (currentGunManager.transform.localPosition != originPos)
        {
            currentGunManager.transform.localPosition = Vector3.Lerp(currentGunManager.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    //총기반동 로직
    IEnumerator RetroAction()
    {
        Vector3 recoilBack = new Vector3 (originPos.x, originPos.y, currentGunManager.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGunManager.fineSinghtOriginPos.x, currentGunManager.fineSinghtOriginPos.y, currentGunManager.retroFineSightForce);

        if (!fineSightCheck)
        {
            currentGunManager.transform.localPosition = originPos;
            if (player_mine.crouchCheck == true)
            {
                camrecoil += 2f;
            }
            else
            {
                camrecoil += 2.8f;
            }
            if(camrecoil != 0)
            {
                StartCoroutine(CamRecoilBack());
            }

            while (currentGunManager.transform.localPosition.z >= currentGunManager.retroActionForce + 0.02f)
            {
                currentGunManager.transform.localPosition = Vector3.Lerp(currentGunManager.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            while(currentGunManager.transform.localPosition != originPos)
            {
                currentGunManager.transform.localPosition = Vector3.Lerp(currentGunManager.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGunManager.transform.localPosition = currentGunManager.fineSinghtOriginPos;
            if (player_mine.crouchCheck == true)
            {
                camrecoil += 0.9f;
            }
            else
            {
                camrecoil += 1.8f;
            }
            if (camrecoil != 0)
            {
                StartCoroutine(CamRecoilBack());
            }

            while (currentGunManager.transform.localPosition.z >= currentGunManager.retroFineSightForce + 0.02f)
            {
                currentGunManager.transform.localPosition = Vector3.Lerp(currentGunManager.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            while (currentGunManager.transform.localPosition != currentGunManager.fineSinghtOriginPos)
            {
                currentGunManager.transform.localPosition = Vector3.Lerp(currentGunManager.transform.localPosition, currentGunManager.fineSinghtOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    IEnumerator CamRecoilBack()
    {
        isRebound = true;
        while (true)
        {
            camrecoil = Mathf.Lerp(camrecoil, 0, Time.deltaTime * 1.5f);
            if(camrecoil < 0.00001)
            {
                camrecoil = 0;
                isRebound = false;

                break;
            }
            yield return null;
        }

        yield break;
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public GunManager GetGun()
    {
        return currentGunManager;
    }

    public bool GetFineSight()
    {
        return fineSightCheck;
    }
}
