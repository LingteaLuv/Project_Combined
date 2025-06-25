using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public MapManager MapManager;
    public Vector3Int StartPos;
    public Vector3Int EndPos;

    private List<AstarNode> _finalPath;
    private List<AstarNode> _searchPath = new List<AstarNode>();
    private int _timeIndex;

    

    public void Find()
    {
        FindPath(StartPos, EndPos);
        _timeIndex = 0;
    }

    public void DrawSearchPath()
    {
        AstarNode node = _searchPath[_timeIndex];
        node.Tile_transform.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0.75f, 1f, 0.5f));
        _timeIndex++;
    }
    void DrawFinalPath()
    {
        foreach (AstarNode node in _finalPath)
        {
            node.Tile_transform.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
    }

    public void FindPath(Vector3Int _startPos, Vector3Int _endPos)
    {
        AstarNode startNode = MapManager.map[_startPos.x, _startPos.y, _startPos.z];
        AstarNode endNode = MapManager.map[_endPos.x, _endPos.y, _endPos.z];

        List<AstarNode> openList = new List<AstarNode>();
        HashSet<AstarNode> closedList = new HashSet<AstarNode>();
        openList.Add(startNode);
        _searchPath.Clear(); // 이전 경로 초기화

        while (openList.Count > 0)
        {
            AstarNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentNode.F || openList[i].F == currentNode.F && openList[i].H < currentNode.H)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            _searchPath.Add(currentNode);

            if (currentNode.Position == endNode.Position)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (AstarNode neighbour in MapManager.GetNeighbour(currentNode))
            {
                if (!neighbour.Walkable || closedList.Contains(neighbour)) continue;

                int newCostToNeighbour = currentNode.G + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.G || !openList.Contains(neighbour))
                {
                    neighbour.G = newCostToNeighbour;
                    neighbour.H = GetDistance(neighbour, endNode);
                    neighbour.Parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }

    public int GetDistance(AstarNode a, AstarNode b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        int dz = Mathf.Abs(a.Z - b.Z);

        int max = Mathf.Max(dx, dy, dz);
        int mid = dx + dy + dz - max - Mathf.Min(dx, dy, dz);
        int min = Mathf.Min(dx, dy, dz);

        return 17 * min + 14 * (mid - min) + 10 * (max - mid);
    }

    public void RetracePath(AstarNode startNode, AstarNode endNode)
    {
        _finalPath = new List<AstarNode>();
        AstarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            _finalPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        _finalPath.Reverse();
    }
}
