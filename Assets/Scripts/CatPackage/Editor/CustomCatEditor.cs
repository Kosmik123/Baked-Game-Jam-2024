﻿using System;
using UnityEditor;
using UnityEngine;

namespace CatPackage.Editor
{
    [CustomEditor(typeof(SOCat))]
    public class CustomCatEditor : UnityEditor.Editor
    {
        private SerializedProperty catSprite;
        private SerializedProperty catName;
        private SerializedProperty catColor;
        private SerializedProperty catEyeColor;
        private SerializedProperty catNoseColor;
        private SerializedProperty catBreed;
        private SerializedProperty attackPrefab;
        private SerializedProperty damage;
        private SerializedProperty cooldown;
        private SerializedProperty health;

        private SOCat _catScript;

        private void OnEnable()
        {
            catSprite = serializedObject.FindProperty("catSprite");
            catName = serializedObject.FindProperty("catName");
            catColor = serializedObject.FindProperty("catColor");
            catEyeColor = serializedObject.FindProperty("catEyeColor");
            catNoseColor = serializedObject.FindProperty("catNoseColor");
            catBreed = serializedObject.FindProperty("catBreed");
            attackPrefab = serializedObject.FindProperty("attackPrefab");
            damage = serializedObject.FindProperty("damage");
            cooldown = serializedObject.FindProperty("cooldown");
            health = serializedObject.FindProperty("health");
            
            _catScript = target as SOCat;
        }

        private int TryGetBreedIndex()
        {
            foreach (var breed in (ECatBreed[])System.Enum.GetValues(typeof(ECatBreed)))
            {
                var sprite = catSprite.objectReferenceValue as Sprite;
                if (sprite == null) continue;
                if(!string.Equals(breed.ToString(), sprite.name, StringComparison.CurrentCultureIgnoreCase))continue;
                return (int)breed;
            }

            return 0;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Display settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(catSprite);
            EditorGUILayout.PropertyField(catName);
            EditorGUILayout.PropertyField(catColor);
            EditorGUILayout.PropertyField(catEyeColor);
            EditorGUILayout.PropertyField(catNoseColor);
            
            catBreed.enumValueIndex = TryGetBreedIndex();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(catBreed);
            EditorGUILayout.EnumPopup("Cat tier", _catScript.GetDisplayInfo().catTier);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space(20);
            
            EditorGUILayout.LabelField("Specific properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(attackPrefab);
            EditorGUILayout.PropertyField(damage);
            EditorGUILayout.PropertyField(cooldown);
            EditorGUILayout.PropertyField(health);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}