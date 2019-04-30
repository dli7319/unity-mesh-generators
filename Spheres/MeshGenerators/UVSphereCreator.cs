using UnityEngine;

public static class UVSphereCreator
{

    private static Vector3[] directions = {
            Vector3.left,
            Vector3.back,
            Vector3.right,
            Vector3.forward
        };

    public static Mesh Create(int heightSegments, int widthSegments, float radius)
    {
        // Longitude |||
        int nbLong = widthSegments;
        // Latitude ---
        int nbLat = heightSegments;

        #region Vertices
        Vector3[] vertices = new Vector3[(nbLong + 1) * (nbLat + 1)];
        float _pi = Mathf.PI;
        float _2pi = _pi * 2f;

        for (int lat = 0; lat <= nbLat; lat++)
        {
            float a1 = _pi * (float)lat / nbLat;
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= nbLong; lon++)
            {
                float a2 = _pi + _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                vertices[lon + lat * (nbLong + 1)] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
            }
        }
        #endregion

        #region Normales		
        Vector3[] normales = new Vector3[vertices.Length];
        for (int n = 0; n < vertices.Length; n++)
            normales[n] = vertices[n].normalized;
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        uvs[0] = Vector2.up;
        uvs[uvs.Length - 1] = Vector2.zero;
        for (int lat = 0; lat <= nbLat; lat++)
            for (int lon = 0; lon <= nbLong; lon++)
                uvs[lon + lat * (nbLong + 1)] = new Vector2((float)lon / nbLong, 1f - (float)lat / nbLat);
        #endregion

        #region Triangles
        int nbFaces = vertices.Length;
        int nbTriangles = nbFaces * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];

        int i = 0;
        //Middle
        for (int lat = 0; lat < nbLat; lat++)
        {
            for (int lon = 0; lon < nbLong; lon++)
            {
                int current = lon + lat * (nbLong + 1);
                int next = current + (nbLong + 1);

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
            }
        }
        #endregion

        Vector4[] tangents = new Vector4[vertices.Length];
        CreateTangents(vertices, tangents);

        Mesh mesh = new Mesh();
        if (vertices.Length > 65535)
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }
        mesh.name = "UV Sphere";
        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.tangents = tangents;
        mesh.triangles = triangles;
        return mesh;
    }

    private static void CreateTangents(Vector3[] vertices, Vector4[] tangents)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];
            v.y = 0f;
            v = v.normalized;
            Vector4 tangent;
            tangent.x = -v.z;
            tangent.y = 0f;
            tangent.z = v.x;
            tangent.w = -1f;
            tangents[i] = tangent;
        }

        //tangents[vertices.Length - 4] = tangents[0] = new Vector3(-1f, 0, -1f).normalized;
        //tangents[vertices.Length - 3] = tangents[1] = new Vector3(1f, 0f, -1f).normalized;
        //tangents[vertices.Length - 2] = tangents[2] = new Vector3(1f, 0f, 1f).normalized;
        //tangents[vertices.Length - 1] = tangents[3] = new Vector3(-1f, 0f, 1f).normalized;
        for (int i = 0; i < 4; i++)
        {
            tangents[vertices.Length - 1 - i].w = tangents[i].w = -1f;
        }
    }
}