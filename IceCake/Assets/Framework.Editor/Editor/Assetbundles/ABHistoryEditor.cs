using UnityEditor;
using UnityEngine;

namespace IceCake.Framework.AssetBundles.Editor
{
    public class ABHistoryEditor : EditorWindow
    {
        private ABHistory abHistory;
        private Vector2 scrollPos;
        private string openFolderPath;

        [MenuItem("Tools/AssetBundle/AssetBundle History")]
        private static void Init()
        {
            var abHistoryEditorWindow = GetWindow<ABHistoryEditor>();
            abHistoryEditorWindow.Show();

            string abOutPath = ABBuilder.Instance.GetPathPrefixAssetBundle();
            abHistoryEditorWindow.abHistory = new ABHistory();
            abHistoryEditorWindow.abHistory.Initialize(abOutPath);
        }

        private void OnEnable()
        {
            string abOutPath = ABBuilder.Instance.GetPathPrefixAssetBundle();
            abHistory = new ABHistory();
            abHistory.Initialize(abOutPath);
        }

        private void OnGUI()
        {
            if (abHistory == null) return;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (var pair in abHistory.Datas)
            {
                var hData = pair.Value;
                if (GUILayout.Button(hData.Time))
                {
                    hData.IsSelected = !hData.IsSelected;
                }
                if (hData.IsSelected)
                {
                    using (var space = new EditorGUILayout.VerticalScope("TextField"))
                    {
                        foreach (var incVerEntryPair in hData.IncVer.Datas)
                        {
                            EditorGUIUtility.labelWidth = 30;

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("AB: ", incVerEntryPair.Value.Name);
                            EditorGUILayout.LabelField("Ver: ", incVerEntryPair.Value.Version.ToString());
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        if (GUILayout.Button("upload ab file", GUILayout.Width(120)))
                        {
                            openFolderPath = EditorUtility.OpenFolderPanel("Upload ab file", openFolderPath, "");
                            if (!string.IsNullOrEmpty(openFolderPath))
                            {
                                string abOutPath = ABBuilder.Instance.GetPathPrefixAssetBundle();
                                hData.IncVer.SaveIncrement(abOutPath, openFolderPath);
                                Debug.Log("Upload success!!");
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
