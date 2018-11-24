using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IceCake.Core.Editor;
using System;
using IceCake.Framework.AssetBundles.Editor;
using IceCake.Core.Json;

namespace UnityEditor.AssetBundles
{
    [Serializable]
    public class AssetBundlePreprocessingTab
    {
        public class EntryData
        {
            public bool IsSettingOpen = false;
            public ABData Data;
        }

        [SerializeField]
        private Vector2 scrollPos;
        private Texture2D refreshTexture;

        private ABDataConfig abEntryConfig;
        //private bool advancedSettings;

        private List<EntryData> entryDatas;

        public void OnEnable(Rect subPos, EditorWindow editorWindow)
        {
            abEntryConfig = EditorAssists.ReceiveAsset<ABDataConfig>(ABBuilder.ABDataConfigPath);
            entryDatas = ToEntryDatas(abEntryConfig);
            refreshTexture = EditorGUIUtility.FindTexture("Refresh");
        }

        public void Update()
        {
        }

        public void OnGUI(Rect rect)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.Space();

            using (var space1 = new EditorGUILayout.VerticalScope())
            {
                using (var space2 = new EditorGUILayout.HorizontalScope("TextField"))
                {
                    EditorGUIUtility.labelWidth = 60;
                    EditorGUILayout.TextField("Target: ", ABBuilder.Instance.CurBuildPlatform.ToString());
                    if (GUILayout.Button(refreshTexture, GUILayout.Width(30)))
                    {
                        abEntryConfig = EditorAssists.ReceiveAsset<ABDataConfig>(ABBuilder.ABDataConfigPath);
                        entryDatas = ToEntryDatas(abEntryConfig);
                    }
                }
                EditorGUILayout.Space();


                for (int i = 0; i < entryDatas.Count; i++)
                {
                    using (var space2 = new EditorGUILayout.VerticalScope("TextField"))
                    {
                        using (var space3 = new EditorGUILayout.HorizontalScope())
                        {
                            entryDatas[i].IsSettingOpen = EditorGUILayout.Foldout(entryDatas[i].IsSettingOpen, entryDatas[i].Data.ABName);
                            if (GUILayout.Button("Del", GUILayout.Width(30)))
                            {
                                entryDatas.RemoveAt(i);
                                return;
                            }
                        }

                        EditorGUILayout.Space();
                        if (entryDatas[i].IsSettingOpen)
                        {
                            EditorGUIUtility.labelWidth = 150;
                            entryDatas[i].Data.ABName = EditorGUILayout.TextField("Assetbundle Name: ", entryDatas[i].Data.ABName);
                            entryDatas[i].Data.ABVariant = EditorGUILayout.TextField("Assetbundle Variant: ", entryDatas[i].Data.ABVariant);
                            entryDatas[i].Data.AssetResPath = EditorGUILayout.TextField("Asset Res Path: ", entryDatas[i].Data.AssetResPath);
                            entryDatas[i].Data.AssetSrcType = (ABData.AssetSourceType)EditorGUILayout.EnumPopup("Asset Src Type: ", entryDatas[i].Data.AssetSrcType);
                            entryDatas[i].Data.AssetType = EditorGUILayout.TextField("Asset Type: ", entryDatas[i].Data.AssetType);
                            entryDatas[i].Data.ABClassName = EditorGUILayout.TextField("Assetbundle Class: ", entryDatas[i].Data.ABClassName);
                            entryDatas[i].Data.ABOriginalResPath = EditorGUILayout.TextField("Assetbundle Original Res: ", entryDatas[i].Data.ABOriginalResPath);

                            // @TODO: 需要编写方便的数组Editor控件
                            //string filterStr = "";
                            //if (mEntryDatas[i].Data.FilterAssets != null) JsonParser.ToJsonNode(mEntryDatas[i].Data.FilterAssets).ToString();
                            //filterStr = EditorGUILayout.TextField("Asset Filter: ", filterStr);
                            //if (!string.IsNullOrEmpty(filterStr))
                            //{
                            //    JsonNode filterJsonNode = JsonParser.Parse(filterStr);
                            //    mEntryDatas[i].Data.FilterAssets = filterJsonNode.ToList<string>();
                            //}
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Add"))
            {
                entryDatas.Add(new EntryData() { IsSettingOpen = false, Data = new ABData() });
            }
            if (GUILayout.Button("Save"))
            {
                abEntryConfig = ToEntryConfig(entryDatas);
                EditorUtility.SetDirty(abEntryConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("Update All Assets AB Labels"))
            {
                ABBuilder.Instance.UpdateAllAssetsABLabels(ABBuilder.ABDataConfigPath);
            }
        }

        public List<EntryData> ToEntryDatas(ABDataConfig dataConfig)
        {
            var entryDatas = new List<EntryData>();
            if (dataConfig == null || dataConfig.ABDatas == null) return entryDatas;

            foreach (var pair in dataConfig.ABDatas)
            {
                var entryData = new EntryData() { IsSettingOpen = false, Data = pair };
                entryDatas.Add(entryData);
            }
            return entryDatas;
        }

        public ABDataConfig ToEntryConfig(List<EntryData> entryDatas)
        {
            ABDataConfig entryConfig = EditorAssists.ReceiveAsset<ABDataConfig>(ABBuilder.ABDataConfigPath);
            if (entryConfig.ABDatas == null) entryConfig.ABDatas = new List<ABData>();
            entryConfig.ABDatas.Clear();
            for (int i = 0; i < entryDatas.Count; i++)
            {
                entryConfig.ABDatas.Add(entryDatas[i].Data);
            }
            return entryConfig;
        }
    }
}
