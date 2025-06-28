using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyInteractionCharacteristics))]
public class EnemyCharacteristicsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("health"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("visionRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("acceleration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("stoppingDistance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isGround"));
        SerializedProperty isGround = serializedObject.FindProperty("isGround");

        if (isGround.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ground Enemy Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minJumpHeight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxJumpHeight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gravity"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
