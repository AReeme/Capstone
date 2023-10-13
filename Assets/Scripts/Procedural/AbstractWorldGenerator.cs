using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWorldGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector3Int startPosition = Vector3Int.zero; // Use Vector3Int for isometric grid

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}