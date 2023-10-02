using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractWorldGenerator), true)]

public class RandomDungeonGeneratorEditor : Editor
{
    AbstractWorldGenerator generator;

    private void Awake()
    {
        generator = (AbstractWorldGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create World"))
        {
            generator.GenerateDungeon();
        }
    }
}
