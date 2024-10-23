using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DungenGenerator.DungeonGenerator))]
    public class DungeonGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DungenGenerator.DungeonGenerator generator = (DungenGenerator.DungeonGenerator)target;

            if (GUILayout.Button("Generate Dungeon"))
            {
                generator.ClearDungeon();
                generator.GenerateDungeon();
            }
        }
    }
}