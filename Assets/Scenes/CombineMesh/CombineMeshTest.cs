using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class CombineMeshTest : MonoBehaviour
{

    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Material[] mats = new Material[meshFilters.Length];
        Matrix4x4 matrix = transform.worldToLocalMatrix;
        for (int i = 0; i < meshFilters.Length; i++)
        {
            MeshFilter mf = meshFilters[i];
            MeshRenderer mr = meshFilters[i].GetComponent<MeshRenderer>();
            if (mr == null)
            {
                continue;
            }
            combine[i].mesh = mf.sharedMesh;
            combine[i].transform = matrix * mf.transform.localToWorldMatrix;
            mr.enabled = false;
            mats[i] = mr.sharedMaterial;
        }
        MeshFilter thisMeshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "Combined";
        thisMeshFilter.mesh = mesh;
        mesh.CombineMeshes(combine, false);
        MeshRenderer thisMeshRenderer = GetComponent<MeshRenderer>();
        thisMeshRenderer.sharedMaterials = mats;
        thisMeshRenderer.enabled = true;

        MeshCollider thisMeshCollider = GetComponent<MeshCollider>();
        if (thisMeshCollider != null)
        {
            thisMeshCollider.sharedMesh = mesh;
        }


        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}