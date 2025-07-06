using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> _npcList;
    public GameObject markerPrefab; // Marker 프리팹 (Inspector에 할당)

    private void Start()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");

        foreach(GameObject npc in npcs)
        {
            GameObject marker = Instantiate(markerPrefab);

            marker.transform.SetParent(npc.transform);

            marker.transform.position = new Vector3(npc.transform.position.x, 250, npc.transform.position.z);
        }
    }
}
