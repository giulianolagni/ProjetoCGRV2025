using UnityEngine;

public class FixCulling : MonoBehaviour
{
    void Start()
    {
        // Aumenta artificialmente a caixa de visibilidade do objeto
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
        }
    }
}