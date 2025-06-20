using System.Collections;
using UnityEngine;

public class MyGunAttackController : MonoBehaviour
{
    [Header("Gun Data")]
    [SerializeField] private MyGun _myGun;  // ��ũ���ͺ� ������Ʈ ����

    private bool _canShot = true;

    private void Awake()
    {
        _myGun= GetComponent<MyGun>();
    }
    private void Update()
    {
        //���߿� �÷��̾� Input���� Shot()
        if (Input.GetKeyDown(KeyCode.X) && _canShot && _myGun.CurrentAmmo > 0)
        {
            Shot();
        }
    }

    //�Ѿ� ������ ����
    private IEnumerator ShotDelay()
    {
        _canShot = false;
        yield return new WaitForSeconds(_myGun.GunData._fireDelay);
        _canShot = true;
    }

    //�ܺο��� ����� ���� ��� �Լ�
    public void Shot()
    {
        if (_myGun.CurrentAmmo  == 0)
        {
            Debug.Log("RŰ�� ���� �����ϼ���");
        }

        //�߻� �� �� �ִ� �Ѿ��� �ִ��� �Ѿ�Ǯ �˻�
        GameObject bulletObj = _myGun.GetBulletPool().GetInactive();

        if (bulletObj != null)  //���� ��� �Դٸ� 
        {
            // �߻� ������ ���� (�Ѿ��� ������ �߻�� ����)
            StartCoroutine(ShotDelay());

            //�Ѿ� ��ġ ����
            bulletObj.transform.position = _myGun.GunData._firePoint.transform.position;

            //�Ѿ� ���� ����
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDirection(_myGun.GunData._firePoint.transform.forward);
            }
            bulletObj.SetActive(true); //�ش� �Ѿ��� Ȱ��ȭ��Ŵ
        }
        else
        {
            Debug.Log("�Ѿ��� �غ�Ǿ� ���� �ʽ��ϴ�");
        }
    }

    private void PlayShootEffects()
    {
        // �ѱ� ȭ�� ����Ʈ
        if (_myGun.GetComponent<ParticleSystem>() != null)
        {
            _myGun.GetComponent<ParticleSystem>().Play();
        }

        // �߻��� ���
        _myGun.PlayFireSound();
    }
}
