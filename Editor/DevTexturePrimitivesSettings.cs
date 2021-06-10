using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace KimMakesGames.DevTexturePrimitives.Editor
{
    public class DevTexturePrimitivesSettings : ScriptableObject
    {
        public const string MaterialPath =
            "Packages/kimmakesgames.devtextureprimitives/Editor/Resources/DevTexturePrimitives/Materials/dev_material.mat";

        private const string DefaultTexturePath = "Packages/kimmakesgames.devtextureprimitives/Editor/Resources/DevTexturePrimitives/Textures/dev_texture.png";
        private const float DefaultScale = 2f;
        private static readonly Color DefaultColor = new Color(1, 0.5f, 0); //this is readonly instead of const bc Color isnt compile time constant
        
        [SerializeField] private Texture2D materialTexture;
        [SerializeField] private float materialScale;
        [SerializeField] private Color materialColor;

        public static Texture2D MaterialTexture { get; private set; }
        public static Color MaterialColor { get; private set; }
        public static float MaterialScale { get; private set; }
        
        private void OnEnable() => Refresh(this);

        public static void Refresh(DevTexturePrimitivesSettings instance)
        {
            MaterialTexture = instance.materialTexture;
            MaterialColor = instance.materialColor;
            MaterialScale = instance.materialScale;

            UpdateMaterial();
        }

        private static void UpdateMaterial()
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            mat.SetColor("_Color", MaterialColor);
            mat.SetTexture("_MainTex", MaterialTexture);
            mat.SetFloat("_Scale", MaterialScale);
        }

        public static void Reset(DevTexturePrimitivesSettings instance)
        {
            instance.materialTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(DefaultTexturePath);
            instance.materialColor = DefaultColor;
            instance.materialScale = DefaultScale;
            
            Refresh(instance);
        }

        private void OnValidate()
        {
            if(materialTexture == null) materialTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(DefaultTexturePath);
            if (materialColor == null) materialColor = DefaultColor;
            if(materialScale == null) materialScale = DefaultScale;
            
            materialScale = Mathf.Max(materialScale, 0.1f);
        }
    }

    static class DevTexturePrimitivesSettingsRegister
    {
        private const string SettingsPath =
            "Packages/kimmakesgames.devtextureprimitives/Editor/Resources/DevTexturePrimitives/Settings/DevTexturePrimitivesSettings.asset";

        private static readonly GUILayoutOption[] _resetButtonOptions = new GUILayoutOption[]
        {
            GUILayout.MaxWidth(100), 
            GUILayout.MaxHeight(25)
        };
        
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Project/Dev Texture Primitives", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    var settings = Load();
                    var serialized = new SerializedObject(settings);

                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.PropertyField(serialized.FindProperty("materialTexture"),
                        new GUIContent("Material Texture"));

                    EditorGUILayout.PropertyField(serialized.FindProperty("materialColor"),
                        new GUIContent("Material Color"));

                    EditorGUILayout.PropertyField(serialized.FindProperty("materialScale"),
                        new GUIContent("Material Scale"));
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        serialized.ApplyModifiedProperties();
                        DevTexturePrimitivesSettings.Refresh(settings);
                    }
                    
                    EditorGUILayout.Separator();

                    
                    if (GUILayout.Button("Reset", _resetButtonOptions))   
                        DevTexturePrimitivesSettings.Reset(settings);
                },

                keywords = new HashSet<string>(new[]
                    {"Material Texture", "Material Color", "Material Scale"})
            };
        }

        private static DevTexturePrimitivesSettings Load()
        {
            var settings = AssetDatabase.LoadAssetAtPath<DevTexturePrimitivesSettings>(SettingsPath);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<DevTexturePrimitivesSettings>();
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }
    }
}