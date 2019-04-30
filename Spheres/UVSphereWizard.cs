using UnityEditor;
using UnityEngine;

public class UVSphereWizard : ScriptableWizard
{

    [MenuItem("Assets/Create/UV Sphere")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<UVSphereWizard>("Create UV Sphere");
    }

    public int heightSegments = 32;
    public int widthSegments = 32;
    public float radius = 0.5f;

    private void OnWizardCreate()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save UV Sphere", "UVSphere" + heightSegments, "asset", "Specify where to save the mesh.");
        if (path.Length > 0)
        {
            Mesh mesh = UVSphereCreator.Create(heightSegments, widthSegments, radius);
            MeshUtility.Optimize(mesh);
            AssetDatabase.CreateAsset(mesh, path);
            Selection.activeObject = mesh;
        }
    }
}