// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// public class MeshCombiner : MonoBehaviour
// {
//     [SerializeField] private List<MeshFilter> sourceMeshFilter;
//     [SerializeField] private MeshFilter targetMeshFilter;

//     [ContextMenu("Combine Meshes")]
//     public void CombineAllMeshes()
//     {
//         MeshFilter[] meshFilters = sourceMeshFilter.ToArray();
//         CombineInstance[] combine = new CombineInstance[meshFilters.Length];

//         for (int i = 0; i < meshFilters.Length; i++)
//         {
//             combine[i].mesh = Instantiate(meshFilters[i].sharedMesh);
//             combine[i].transform = meshFilters[i].transform.worldToLocalMatrix * targetMeshFilter.transform.localToWorldMatrix;
//         }

//         var mesh = new Mesh();
//         mesh.CombineMeshes(combine);
//         targetMeshFilter.transform.position = Vector3.zero;
//         targetMeshFilter.transform.rotation = Quaternion.identity;
//         targetMeshFilter.transform.localScale = Vector3.one;

//         Debug.Log("VERTS = " + mesh.vertexCount);

//         mesh.RecalculateBounds();
//         mesh.RecalculateNormals();
//         mesh.RecalculateTangents();

//         targetMeshFilter.mesh = mesh;
//         mesh.name = "CombinedMesh";
        
//         AssetDatabase.CreateAsset(mesh, "Assets/CombinedMesh.asset");

//         Debug.Log("Meshes combined! and saved in Assets/CombinedMesh.asset");
//     }
// }