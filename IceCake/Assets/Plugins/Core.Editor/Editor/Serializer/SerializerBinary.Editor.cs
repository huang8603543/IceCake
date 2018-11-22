using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using UnityEngine;

namespace IceCake.Core.Serializer.Editor
{
    public class SerializeBinaryEditor : TSingleton<SerializeBinaryEditor>
    {
        CodeGeneratorCommonSerializer commonSerializer;
        List<CodeGeneratorClassSerializer> classSerializers;

        private SerializeBinaryEditor()
        { }

        [MenuItem("Tools/Serializer/Auto CS Code Generate...")]
        public static void CodeGenerate()
        {
            string generatePathRoot = "Assets/Framework/Generate/SerializerBinary/";
            Instance.Analysis(generatePathRoot, "Framework", (text, progress) => 
            {
                EditorUtility.DisplayProgressBar("AutoCSGenerate", text, progress);
            });

            generatePathRoot = "Assets/Game/Metal/Script/Generate/SerializerBinary/";
            Instance.Analysis(generatePathRoot, "Game", (text, progress) =>
            {
                EditorUtility.DisplayProgressBar("AutoCSGenerate", text, progress);
            });
            EditorUtility.ClearProgressBar();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Auto serialize cs generate complete..");
        }

        public void Analysis(string generatePathRoot, string dllModuleName, Action<string, float> progressAction = null)
        {
            string generatePath = generatePathRoot;
            string commonSerializerPath = generatePath + "CommonSerializer.cs";

            classSerializers = new List<CodeGeneratorClassSerializer>();

            commonSerializer = new CodeGeneratorCommonSerializer(commonSerializerPath);
            commonSerializer.WriteHead();

            var sbTypes = SerializerBinaryTypes.Types;
            for (int i = 0; i < sbTypes.Count; i++)
            {
                if (!sbTypes[i].Assembly.GetName().Name.Equals(dllModuleName))
                    continue;

                var groupName = string.Empty;
                var attributes = sbTypes[i].GetCustomAttributes<SBGroupAttribute>(true);
                if (attributes.Length > 0)
                    groupName = attributes[0].GroupName;
                var classSerializer = new CodeGeneratorClassSerializer(UtilTool.PathCombine(generatePath, groupName, sbTypes[i].FullName + ".Binary.cs"));
                classSerializer.WriteHead();
                classSerializer.WriteClass(sbTypes[i]);
                classSerializer.WriteEnd();

                classSerializers.Add(classSerializer);

                var serializeMemberInfo = SerializerAssists.FindSerializeMembers(sbTypes[i]);
                foreach (var memberInfo in serializeMemberInfo)
                {
                    var dynamic = memberInfo.IsDefined(typeof(SBDynamicAttribute), false);
                    if (memberInfo.MemberType == MemberTypes.Field)
                        commonSerializer.AnalyzeGenerateCommon((memberInfo as FieldInfo).FieldType, dynamic);
                    else if (memberInfo.MemberType == MemberTypes.Property)
                        commonSerializer.AnalyzeGenerateCommon((memberInfo as PropertyInfo).PropertyType, dynamic);
                }
                UtilTool.SafeExcute(progressAction, $"Generate File: {sbTypes[i].FullName}", i / (float)sbTypes.Count);
                classSerializer.Save();
            }
            commonSerializer.WriteEnd();
            commonSerializer.Save();
        }
    }
}

