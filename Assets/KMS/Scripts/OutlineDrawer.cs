using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class OutlineDrawer : MonoBehaviour
{
    public Color outlineColor = Color.green;
    public bool enabled = true;

    void OnRenderObject()
    {
        if (enabled) return;

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) return;

        Material mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        mat.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        GL.Color(outlineColor);

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // edge를 해시로 저장
        Dictionary<Edge, int> edgeCount = new Dictionary<Edge, int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            AddEdge(edgeCount, new Edge(triangles[i], triangles[i + 1]));
            AddEdge(edgeCount, new Edge(triangles[i + 1], triangles[i + 2]));
            AddEdge(edgeCount, new Edge(triangles[i + 2], triangles[i]));
        }

        // 한 번만 등장한 edge만 외곽선으로 간주
        foreach (var pair in edgeCount)
        {
            if (pair.Value == 1)
            {
                Edge e = pair.Key;
                GL.Vertex(vertices[e.v1]);
                GL.Vertex(vertices[e.v2]);
            }
        }

        GL.End();
        GL.PopMatrix();
    }

    void AddEdge(Dictionary<Edge, int> dict, Edge edge)
    {
        if (dict.ContainsKey(edge))
            dict[edge]++;
        else
            dict[edge] = 1;
    }

    struct Edge
    {
        public int v1;
        public int v2;

        public Edge(int a, int b)
        {
            // 항상 작은 수 먼저 오게 정렬 (같은 edge로 인식되게)
            if (a < b) { v1 = a; v2 = b; }
            else { v1 = b; v2 = a; }
        }

        public override int GetHashCode() => v1 * 73856093 ^ v2 * 19349663;
        public override bool Equals(object obj)
        {
            if (!(obj is Edge)) return false;
            Edge other = (Edge)obj;
            return v1 == other.v1 && v2 == other.v2;
        }
    }
}