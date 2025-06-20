using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttackController : MonoBehaviour
{
    [SerializeField] private Gun _currentGun; //Gun�� gunAttackConrtoller�� ����
    [SerializeField] private Transform _firePoint; //�Ѿ� �߻� ����
    [SerializeField] private float _fireDelay = 1.5f; //�� �߻� ������

    [Space(10)]
    [Header("Legacy Information")]
    [Tooltip("���� ���� �����ִ� ź��� ,���߿� �κ��丮���� ���;��ϴ� ���� ���� �˼��� ����")]
    [SerializeField] private int _currentAmmo = 30; 

    private bool _canShot = true;

    private void Start()
    {
        if(!_currentGun)
        {
            _currentGun = gameObject.GetComponent<Gun>();
        }
    }

    private void Update()
    {
        //���߿� �÷��̾� Input���� Shot()
        if(Input.GetKeyDown(KeyCode.X) && _canShot && _currentAmmo > 0)
        {
            Shot();
        }
    }

    //�Ѿ� ������ ����
    private IEnumerator ShotDelay()
    {
        _canShot = false;
        yield return new WaitForSeconds(_fireDelay);
        _canShot = true;
    }

    //�ܺο��� ����� ���� ��� �Լ�
    public void Shot()
    {
        if (_currentAmmo == 0)
        {
            Debug.Log("RŰ�� ���� �����ϼ���");
        }

        //�߻� �� �� �ִ� �Ѿ��� �ִ��� �Ѿ�Ǯ �˻�
        GameObject bulletObj = _currentGun._gunBulletObjectPool.GetInactive();

        if (bulletObj != null)  //���� ��� �Դٸ� 
        {
            // �߻� ������ ���� (�Ѿ��� ������ �߻�� ����)
            StartCoroutine(ShotDelay());

            //�Ѿ� ��ġ ����
            bulletObj.transform.position = _firePoint.transform.position;

            //�Ѿ� ���� ����
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDirection(_firePoint.forward);
            }
            bulletObj.SetActive(true); //�ش� �Ѿ��� Ȱ��ȭ��Ŵ
        }
        else
        {
            Debug.Log("�Ѿ��� �غ�Ǿ� ���� �ʽ��ϴ�");
        }
    }
}
