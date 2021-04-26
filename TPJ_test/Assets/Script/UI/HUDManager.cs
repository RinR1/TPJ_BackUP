using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private GunController gc_mine;
    private GunManager gm_mine;

    // HUD 온오프용
    [SerializeField]
    private GameObject bulletHud;

    [SerializeField]
    private Text[] t_Bullet;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    // Update is called once per frame
    void Update()
    {
        BulletCheck();
    }

    private void BulletCheck()
    {
        gm_mine = gc_mine.GetGun();
        t_Bullet[0].text = gm_mine.currentBulletCount.ToString();
        t_Bullet[1].text = gm_mine.carryBulletCount.ToString();
    }
}
