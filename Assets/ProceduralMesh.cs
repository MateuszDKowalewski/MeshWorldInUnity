using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class ProceduralMesh : MonoBehaviour
{

    public int width = 21;
    public int deapth = 21;

    private Mesh mesh;
    private MeshCollider meshCollider;

    private Vector3[] vertices;
    private int[] triangles;

    private float[,] heights;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = gameObject.GetComponent<MeshCollider>();

        heights = new float[width,deapth];

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[width * deapth];
        triangles = new int[(width - 1) * (deapth - 1) * 6];


        for(int x = 0; x < width; x++)
        {
            for (int z = 0; z < deapth; z++)
            {
                float averg = 0;
                int amount = 0;

                if(x > 0)
                {
                    averg += heights[x - 1, z];
                    amount++;
                }

                if (z > 0)
                {
                    averg += heights[x, z - 1];
                    amount++;
                }

                if(amount > 0)
                {
                    averg /= amount;
                }
                else
                {
                    averg = 5;
                }

                heights[x, z] = averg + Random.Range(-.2f, .2f);
                vertices[z * width + x] = new Vector3(x - width / 2, heights[x, z], z - deapth / 2);
            }
        }

        int counter = 0;
        for(int x = 0; x < width - 1; x++)
        {
            for(int z = 0; z < deapth - 1; z++)
            {
                triangles[counter++] = z * width + x;
                triangles[counter++] = (z + 1) * width + x;
                triangles[counter++] = z * width + (x + 1);
                triangles[counter++] = z * width + (x + 1);
                triangles[counter++] = (z + 1) * width + x;
                triangles[counter++] = (z + 1) * width + (x + 1);
            }
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        meshCollider.sharedMesh = mesh;
        mesh.RecalculateNormals();
    }
}
