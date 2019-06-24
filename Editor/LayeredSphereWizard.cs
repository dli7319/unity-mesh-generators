using UnityEditor;
using UnityEngine;
using System.Threading;

public class LayeredSphereWizard : ScriptableWizard
{
    [MenuItem("Assets/Create/Layered Sphere")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<LayeredSphereWizard>("Create Layered Sphere");
    }

    public int level = 6;
    public float minRadius = 0.5f;
    public float maxRadius = 0.6f;
    public int numberOfSpheres = 2;

    private void OnWizardCreate()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Layered Sphere", "LayeredSphere" + level + "_" + numberOfSpheres, "asset", "Specify where to save the mesh.");
        if (path.Length > 0)
        {
            Mesh mesh = new Mesh();

            Mesh temp = OctahedronSphereCreator.Create(level, minRadius);
            int tempVerticesLength = temp.vertices.Length;
            int tempTrianglesLength = temp.triangles.Length;
            Vector3[] vertices = new Vector3[numberOfSpheres * tempVerticesLength];
            Vector3[] normals = new Vector3[numberOfSpheres * tempVerticesLength];
            Vector2[] uv = new Vector2[numberOfSpheres * tempVerticesLength];
            Vector4[] tangents = new Vector4[numberOfSpheres * tempVerticesLength];
            int[] triangles = new int[numberOfSpheres * tempTrianglesLength];
            Vector3[] tempVertices = temp.vertices;
            Vector3[] tempNormals = temp.normals;
            Vector2[] tempUv = temp.uv;
            Vector4[] tempTangents = temp.tangents;
            int[] tempTriangles = temp.triangles;

            for (int i = 0; i < numberOfSpheres; i++)
            {

                float thisRadius = minRadius + ((float)i / (numberOfSpheres - 1)) * (maxRadius - minRadius);

                temp = OctahedronSphereCreator.Create(level, thisRadius);
                tempVertices = temp.vertices;
                tempNormals = temp.normals;
                tempUv = temp.uv;
                tempTangents = temp.tangents;
                tempTriangles = temp.triangles;

                System.Array.Copy(tempVertices, 0, vertices, i * tempVerticesLength, tempVerticesLength);
                System.Array.Copy(tempNormals, 0, normals, i * tempVerticesLength, tempVerticesLength);
                System.Array.Copy(tempUv, 0, uv, i * tempVerticesLength, tempVerticesLength);
                System.Array.Copy(tempTangents, 0, tangents, i * tempVerticesLength, tempVerticesLength);
                System.Array.Copy(tempTriangles, 0, triangles, i * tempTrianglesLength, tempTrianglesLength);
                for (int j = 0; j < tempTrianglesLength; j++)
                {
                    triangles[i * tempTrianglesLength + j] += i * tempVerticesLength;
                }
            }

            if (vertices.Length > 65535)
            {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.tangents = tangents;
            mesh.triangles = triangles;

            // Do not optimize the geometry or index buffers here.
            // Optimizing changes the order of the vertices
            // which will alter the draw order causing
            // issues with transparency.
            mesh.OptimizeReorderVertexBuffer();
            AssetDatabase.CreateAsset(mesh, path);
            Selection.activeObject = mesh;
        }
    }
}
