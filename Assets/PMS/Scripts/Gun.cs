using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour //���� �Ѹ��� ����� ���� ��� ���� ������
{
    [Header("Gun Setting")]
    [Tooltip("Change value by gun type")]
    [SerializeField] private string _gunName;  // ���� �̸�
    [SerializeField] private float _range;     // ���� ���� �Ÿ�    
    [SerializeField] public float _reloadTime;// ������ �ӵ�. ���� �������� �ٸ�.
    [SerializeField] private Transform muzzlePoint;

    [Space(100)]
    [Tooltip("Gun Damage value")]
    public int _damage;      // ���� ���ݷ�. ���� �������� �ٸ�.

    //���߿� �߰��� �� ���� �͵�
    //[SerializeField] private float accuracy;  // ���� ��Ȯ��. ���� �������� ��Ȯ���� �ٸ�.
    //[SerializeField] private float fireRate;  // ���� �ӵ�. �� �ѹ߰� �ѹ߰��� �ð� ��. ������ ���� ���� ���簡 ������. ���� �������� �ٸ�.
    //[SerializeField] private float retroActionForce;  // �ݵ� ����. ���� �������� �ٸ�.


    [Header("Gun Bullet Setting")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _bulletPoolSize; //�� �ִ� ź�� ���� ���� �� ������Ʈ Ǯ ������  

    [Header("Gun Effects Setting")]
    [Tooltip("Gun Other Setting")]
    [SerializeField] private ParticleSystem muzzleFlash;  // ȭ���� ����Ʈ ����� ����� ��ƼŬ �ý��� ������Ʈ
    [SerializeField] private AudioClip fire_Sound;    // �� �߻� �Ҹ� ����� Ŭ��
    
    public ObjectPool _gunBulletObjectPool;


    private void Awake()
    {
        Init();     //���߿� �÷��̾� �ش� �� ������Ʈ�� ���� ���� �� Init�� �ǰ� �ؾ��� �� ����.
    }
    private void Init()
    {
        _gunBulletObjectPool = new ObjectPool(_bulletPoolSize, _bulletPrefab, gameObject);
    }
}
