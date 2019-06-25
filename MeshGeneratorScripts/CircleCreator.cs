using UnityEngine;

public static class CircleCreator
{
    public static readonly int minimumSegments = 3;
    public static readonly int maximumSegments = 4096;

    public static Mesh Create(int segments, float radius, bool flip)
    {
        if (segments < minimumSegments)
        {
            segments = minimumSegments;
            Debug.Log($"Segments increased to the minimum of {minimumSegments}");
        }
        if (segments > maximumSegments)
        {
            segments = maximumSegments;
            Debug.Log($"Segments decreased to the maximum of {maximumSegments}");
        }

        #region Vertices
        Vector3[] vertices = new Vector3[2 * segments + 1];
        for (int i = 0; i < segments; i++)
        {
            vertices[i] = Vector3.zero;
        }
        for (int i = 0; i <= segments; i++)
        {
            float angle = 2f * (((float)i) / segments) * Mathf.PI;
            vertices[i + segments] = new Vector3(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle), 0);
        }
        #endregion

        #region Normals		
        Vector3[] normals = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            normals[i] = Vector3.forward;
        }
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * ((float)i / segments);
            uvs[i] = new Vector2(angle, 0f);
        }
        for (int i = 0; i <= segments; i++)
        {
            float angle = 2 * ((float)i / (segments + 1));
            uvs[i + segments] = new Vector2(angle, 1f);
        }
        #endregion

        #region Triangles
        int[] triangles = new int[3 * segments];
        for (int i = 0; i < segments; i++)
        {
            if (flip)
            {
                triangles[3 * i] = i;
                triangles[3 * i + 1] = i + segments + 1;
                triangles[3 * i + 2] = i + segments;
            }
            else
            {
                triangles[3 * i] = i;
                triangles[3 * i + 1] = i + segments;
                triangles[3 * i + 2] = i + segments + 1;
            }
        }
        #endregion


        Mesh mesh = new Mesh();
        if (vertices.Length > 65535)
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }
        mesh.name = "Circle";
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        return mesh;
    }
}