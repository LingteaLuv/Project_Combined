// 1. �ѱ� �����͸� ��� ScriptableObject ����
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Data", menuName = "Weapons/Gun Data")]
public class GunData : ScriptableObject
{
    [SerializeField] public GameObject gunPrefab; //���� ������

    [Header("Gun Setting")]
    [Tooltip("Change gun performance value by gun type")]
    [SerializeField] public string _gunName;  // ���� �̸�
    [SerializeField] public float _damage;    //���� ������
    [SerializeField] public float _fireDelay = 0.1f; // �߻� ����
    [SerializeField] public float _reloadTime;// ������ �ӵ�. ���� �������� �ٸ�.

    //���߿� �߰��� �� ���� �͵�
    //[SerializeField] private float accuracy;  // ���� ��Ȯ��. ���� �������� ��Ȯ���� �ٸ�.
    //[SerializeField] private float fireRate;  // ���� �ӵ�. �� �ѹ߰� �ѹ߰��� �ð� ��. ������ ���� ���� ���簡 ������. ���� �������� �ٸ�.
    //[SerializeField] private float retroActionForce;  // �ݵ� ����. ���� �������� �ٸ�.

    [Header("Gun Bullet Setting")]
    [SerializeField] public GameObject _bulletPrefab; //�Ѿ� ������
    [SerializeField] public Transform _firePoint; //�Ѿ� �߻� ����
    [SerializeField] public int _maxLoadedAmmo; //�ִ� ���� �� �� �ִ� ź���
    [SerializeField] public float _bulletSpeed;  //�Ѿ� ���ǵ�
    [SerializeField] public float _bulletrange;     // �Ѿ� ��ȿ ��Ÿ�    


    [Header("Gun Effects Setting")]
    [SerializeField] public ParticleSystem _muzzleFlash;  // ȭ���� ����Ʈ ����� ����� ��ƼŬ �ý��� ������Ʈ
    [SerializeField] public AudioClip _fireSound;    // �� �߻� �Ҹ� ����� Ŭ��
    [SerializeField] public AudioClip _reloadSound;  // ���� ���ε� ����
    [SerializeField] public AudioClip _emptySound;  //�� źâ��

    [SerializeField] public int _bulletPoolSize; //�߻� �� �� �ִ� �� �ִ� ź�� ���� ���� �� ������Ʈ Ǯ ������  
    private ObjectPool _gunBulletObjectPool;
    public Property<ObjectPool> GunBulletObjectPool;
}
