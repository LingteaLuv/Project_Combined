using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public GameObject shadowPlane;
    public Transform player;
    public LayerMask shadowLayer;
    public float shadowRadius = 10.0f;
    private float radiusCircle { get { return shadowRadius * shadowRadius; } }

    private Mesh mesh;
    private Vector3[] vectices;
    private Color[] colors;


    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //카메라 -> 플레이어 방향으로 레이 쏘기
        Ray r = new Ray(transform.position, player.position - transform.position);
        RaycastHit hit;
        //맞은 객체가 shadowLayer레이러를 가지고 있고 트리거든 콜라이더든 상관없이 찾아낸다.
        if (Physics.Raycast(r,out hit, 1000, shadowLayer, QueryTriggerInteraction.Collide))
        {
            //Debug.Log("RAY");
            //모든 정점을 순회?
            for(int i = 0; i < vectices.Length; i++)
            {
                //공간에 있는 정점(vectices[i])의 좌표를 월드(World) 공간 좌표로 변환
                Vector3 v = shadowPlane.transform.TransformPoint(vectices[i]);
                float distance = Vector3.SqrMagnitude(v - hit.point);
                if(distance < radiusCircle)
                {
                    float alpha = Mathf.Min(colors[i].a, distance / radiusCircle);
                    colors[i].a = alpha;
                }
            }

            UpdateColors();
        }
    }

    public void UpdateColors()
    {
        mesh.colors = colors;
    }

    private void Initialize()
    {
        mesh = shadowPlane.GetComponent<MeshFilter>().mesh;
        vectices = mesh.vertices;
        colors = new Color[vectices.Length];
        for(int i =0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }
        UpdateColors();
    }
}
