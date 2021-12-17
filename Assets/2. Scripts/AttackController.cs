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

    private void GunFireRateCalc() // ����ӵ� �� 0�̿��� �߻縦 �� �� ����
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire() // ���⿡ ������ ���콺 ���� ������ �ɷ���.
    {
        if (isfineSightMode) // �������϶��� ���ݰ���.
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
        currentFireRate = currentGun.fireRate; //����ӵ� ����(�ٽ� �������� 0.2�� ���ư�)
        // ���⿡ �ݵ� �ִϸ��̼� �߰�.
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
            Debug.Log("����ź�� ����");
        }
    }
    
    // ������ �õ�
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // ������ ���
    public void CancelFineSight()
    {
        if (isfineSightMode)
            FineSight();
    }

    // ������
    private void FineSight()
    {
        isfineSightMode = !isfineSightMode;
        currentGun.anim.SetBool("FineSightMode", isfineSightMode);
    }


    
    private void PlaySE(AudioClip _clip) // ����� ���忡 ���� �Լ�.
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Attack GetAttack()
    {
        return currentGun;
    }
}
