using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator crossAnim;

    private float gunAcoaccuracy;

    //크로스헤어 활성, 비활성화용
    [SerializeField]
    private GameObject g_CrosshairHUD;
    [SerializeField]
    private GunController gc_mine;


    public void WalkingAnimation(bool _flag)
    {
        crossAnim.SetBool("Walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        crossAnim.SetBool("Running", _flag);
    }

    public void CrouchingAnimation(bool _flag)
    {
        crossAnim.SetBool("Crouching", _flag);
    }

    public void FineSightAnimation(bool _flag)
    {
        crossAnim.SetBool("FineSight", _flag);
    }

    public void FireAnimation()
    {
        if (crossAnim.GetBool("Walking"))
            crossAnim.SetTrigger("Walk_Shot");
        else if (crossAnim.GetBool("Crouching"))
            crossAnim.SetTrigger("Crouch_Shot");
        else
            crossAnim.SetTrigger("Idle_Shot");
    }

    public float GunAccuracy()
    {
        if (crossAnim.GetBool("Walking"))
            gunAcoaccuracy = 0.06f;
        else if (crossAnim.GetBool("Crouching"))
            gunAcoaccuracy = 0.015f;
        else if(gc_mine.GetFineSight())
            gunAcoaccuracy = 0.001f;
        else
            gunAcoaccuracy = 0.035f;

        return gunAcoaccuracy;
    }
}
