using System;
using UnityEngine;

namespace UnityTemplateProjects
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private int density = 128;
        [SerializeField] private float spacing = .5f;
        [SerializeField] private NoiseSettings noiseSettings;
        [SerializeField] private bool alwaysRegen;

        private MeshFilter filter;
        private MeshRenderer renderer;

        private void Start()
        {
            filter = gameObject.GetComponent<MeshFilter>();
            renderer = gameObject.GetComponent<MeshRenderer>();

            float[,] noiseMap = NoiseFunctions.GenerateNoiseMap(density, noiseSettings);

            filter.mesh = MeshGenerator.Generate(density, noiseMap, spacing);
        }

        private void Update()
        {
            if (alwaysRegen)
            {
                float[,] noiseMap = NoiseFunctions.GenerateNoiseMap(density, noiseSettings);

                filter.mesh = MeshGenerator.Generate(density, noiseMap, spacing);
            }
        }
    }
}