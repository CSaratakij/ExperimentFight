
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ExperimentFight.Editor
{
    public class SpriteMaterialSelector : EditorWindow
    {
        [SerializeField]
        Material material;


        SpriteRenderer[] spriteRenderers;


        [MenuItem("Custom/SpriteMaterialSelector")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(SpriteMaterialSelector));
        }

        void OnGUI()
        {
            titleContent.text = "SpriteMaterialSelector";
            GUILayout.Label ("Setting", EditorStyles.boldLabel);
            
            material = (Material)EditorGUILayout.ObjectField(
                new GUIContent(""),
                material,
                typeof(Material),
                false);

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply"))
                ApplyMaterial();

        }

        void ApplyMaterial()
        {
            if (material == null)
                return;

            spriteRenderers = Object.FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];

            if (spriteRenderers.Length <= 0)
                return;

            foreach (SpriteRenderer sprite in spriteRenderers) {
                Undo.RecordObject(sprite, "Apply sprite's material");
                sprite.material = material;
            }
        }
    }
}

