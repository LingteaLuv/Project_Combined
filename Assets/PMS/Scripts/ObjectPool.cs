using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject[] _pool;
    
    //�����ڸ� ���� ������ƮǮ ũ��,������ ������Ʈ(������),��� ������ ���� �θ� ������Ʈ ���ϱ�
   public ObjectPool(int size, GameObject target, GameObject parent)
    {
        CreatePool(size, target, parent);
    }

    //�ܺο��� ����� Ư�� ������Ʈ Ǯ ���� �Լ�
    public void CreatePool(int size, GameObject target, GameObject parent)
    {
        _pool = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            GameObject obj = MonoBehaviour.Instantiate(target, parent.transform);
            _pool[i] = obj;
            _pool[i].SetActive(false);
        }
    }

    //flag���� ���� Ǯ�� �����ϴ� ��� ������Ʈ Ȱ��ȭ,��Ȱ��ȭ ��Ű�� �Լ�
    public void Activate(bool flag)
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            if (_pool[i].activeSelf != flag)
            {
                _pool[i].SetActive(flag);
                return;
            }
        }
    }

    //��Ȱ��ȭ�� ��� ������ ��ü ã��
    public GameObject GetInactive()
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            if (!_pool[i].activeSelf) // ��Ȱ��ȭ�� ��ü ã��
            {
                return _pool[i]; // ã���� �ش� ��ü ��ȯ
            }
        }
        return null; // ��� ������ ��ü�� ������ null ��ȯ
    }

    //�迭�� ��ȸ�Ͽ� ���� destroy��Ű���Լ�
    public void DestroyAll()
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            MonoBehaviour.Destroy(_pool[i]);
        }
        _pool = null;   //������Ʈ Ǯ �ʱ�ȭ
    }


}
