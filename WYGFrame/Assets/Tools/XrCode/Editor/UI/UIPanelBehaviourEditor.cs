using System;
using UnityEditor;
using UnityEngine;

namespace XrCode
{
    [CustomEditor(typeof(UIPanelBehaviour))]
    public class UIPanelBehaviourEditor : UIViewBehaviourEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight / 4);
            DrawOpElementList();
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
            DrawExpoertButton();

            serializedObject.ApplyModifiedProperties();
        }
    }
}