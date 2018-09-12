
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ExperimentFight.Editor
{
    public class SpriteMaterialSelector : EditorWindow
    {
        [SerializeField]
        int currentSelectSortingLayerIndex = 0;

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
            string[] sortingLayerNames = new string[SortingLayer.layers.Length];

            for (int i = 0; i < sortingLayerNames.Length; i++) {
                int id = SortingLayer.layers[i].id;
                sortingLayerNames[i] = SortingLayer.IDToName(id);
            }

            GUILayout.Label ("Setting", EditorStyles.boldLabel);
            currentSelectSortingLayerIndex = EditorGUILayout.Popup("Target Layer", currentSelectSortingLayerIndex, sortingLayerNames);

            EditorGUILayout.Space();

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

            bool isConfirm = EditorUtility.DisplayDialog("Apply SpriteRender's material", "Are you sure to apply a new material on selected layer?", "Yes", "No");

            if (!isConfirm)
                return;

            foreach (SpriteRenderer sprite in spriteRenderers) {
                if (sprite.sortingLayerID != SortingLayer.layers[currentSelectSortingLayerIndex].id)
                    continue;

                Undo.RecordObject(sprite, "Apply sprite's material");
                sprite.material = material;
            }
        }
    }
}

