using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GeneratedQuad : MonoBehaviour
{
    private MeshFilter filter;
    private new MeshRenderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        renderer = gameObject.GetComponent<MeshRenderer>();

        Vector3[] verts = new Vector3[]
        {
            Vector3.up,
            Vector3.up + Vector3.right,
            Vector3.zero,
            Vector3.right
        };

        int[] tris =
        {
            0,1,3,
            0,3,2
        };

        Vector3[] normals =
        {
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            Vector3.forward
        };

        Vector2[] uvs =
        {
            new(0,0),
            new(1,0),
            new(0,1),
            new(1,1),
            
        };

        Color[] colors =
        {
            Color.green,
            Color.red,
            Color.blue,
            Color.yellow
        };

        Mesh mesh = new()
        {
            vertices = verts,
            uv = uvs,
            normals = normals,
            triangles = tris,
            colors = colors
        };
        
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        filter.mesh = mesh;

        Texture2D texture = new(64, 64, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        
        bool isGrey = true;
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if ((y * texture.width + x) % 4 == 0)
                {
                    isGrey = !isGrey;
                }

                Color color = isGrey ? Color.grey : Color.red;
                texture.SetPixel(x,y,color);
            }
        }
        texture.Apply();
        renderer.material.mainTexture = texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
