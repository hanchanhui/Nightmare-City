using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    private Attack currentGun;

    private float currentFireRate;

    private bool isReload = false;
    private bool isfineSightMode = false;

    private AudioSource audioSource;

    private RaycastHit hitInfo;
    private MonsterCtrl themonsterCtrl;

    [SerializeField]
    private Camera theCam;
    //private Crosshair thecrosshair;

    public int damage = 30;

    void Start()
    {
        themonsterCtrl = GetComponent<MonsterCtrl>();
        audioSource = GetComponent<AudioSource>();
        //thecrosshair = FindObjectOfType<Crosshair>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    private void GunFireRateCalc() // 연사속도 가 0이여야 발사를 할 수 있음
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire() // 여기에 오른쪽 마우스 값을 넣으면 될려나.
    {
        if (isfineSightMode) // 정조준일때만 공격가능.
        {
            if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroution());
            }
        }
    }

    private void Shoot()
    {
        //thecrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; //연사속도 재계산(다시 설정해준 0.2로 돌아감)
        // 여기에 반동 애니메이션 추가.
        PlaySE(currentGun.fire_Sound);
        Hit();
    }

    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward //+
            //new Vector3(Random.Range(-thecrosshair.GetAccuracy() - currentGun.accuracy, thecrosshair.GetAccuracy() + currentGun.accuracy),
            //            Random.Range(-thecrosshair.GetAccuracy() - currentGun.accuracy, thecrosshair.GetAccuracy() + currentGun.accuracy),
            //            0)
            , out hitInfo, currentGun.range))
        {
            if (hitInfo.transform.tag == "BulletSpawner")
            {
                hitInfo.transform.GetComponent<MonsterCtrl>().TakeDamage(damage);
                //Debug.Log(hitInfo.transform.name);
            }

        }
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroution());
        }
    }

    IEnumerator ReloadCoroution()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");


            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.currentBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("남은탄이 없음");
        }
    }
    
    // 정조준 시도
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // 정조준 취소
    public void CancelFineSight()
    {
        if (isfineSightMode)
            FineSight();
    }

    // 정조준
    private void FineSight()
    {
        isfineSightMode = !isfineSightMode;
        currentGun.anim.SetBool("FineSightMode", isfineSightMode);
    }


    
    private void PlaySE(AudioClip _clip) // 오디오 사운드에 관한 함수.
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Attack GetAttack()
    {
        return currentGun;
    }
}
