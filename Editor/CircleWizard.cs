using UnityEditor;
using UnityEngine;

public class CircleWizard : ScriptableWizard
{

    [MenuItem("Assets/Create/Circle")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CircleWizard>("Create Circle");
    }

    public int segments = 32;
    public float radius = 0.5f;
    public bool flip = false;

    private void OnWizardCreate()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Circle", $"Circle_{segments}_{radius}{(flip ? "_flip" : "")}", "asset", "Specify where to save the mesh.");
        if (path.Length > 0)
        {
            Mesh mesh = CircleCreator.Create(segments, radius, flip);
            MeshUtility.Optimize(mesh);
            AssetDatabase.CreateAsset(mesh, path);
            Selection.activeObject = mesh;
        }
    }
}
