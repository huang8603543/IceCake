using System.Text;
using System.IO;
using UnityEditor;

namespace IceCake.Core.Serializer.Editor
{
    public class CodeGenerator
    {
        public const string CSMD5Path = "Library/CSGenerate";

        public StringBuilder StringBuilder;
        public string FilePath;
        public string MD5;
        public string GUID;
        public bool NeedReImport;

        public CodeGenerator(string filePath)
        {
            FilePath = filePath;
            StringBuilder = new StringBuilder();
        }

        public virtual void WriteHead()
        { }

        public virtual void WriteEnd()
        { }

        public void Write()
        {
            StringBuilder?.Line();
        }

        public void Write(int tabCount, string content)
        {
            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(content);
        }

        public void Write(int tabCount, string content, params object[] args)
        {
            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(string.Format(content, args));
        }

        public void WriteBraceCode(int tabCount, string headContent, string leftBrace, string content, string rightBrace)
        {
            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(headContent);

            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(leftBrace);

            StringBuilder?.Tab(tabCount + 1);
            StringBuilder?.AppendLine(content);

            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(rightBrace);
        }

        public string GenernateBraceCode(int tabCount, string headContent, string leftBrace, string content, string rightBrace)
        {
            StringBuilder stringbuilder = new StringBuilder();

            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(headContent);

            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(leftBrace);

            StringBuilder?.Tab(tabCount + 1);
            StringBuilder?.AppendLine(content);

            StringBuilder?.Tab(tabCount);
            StringBuilder?.AppendLine(rightBrace);

            return stringbuilder.ToString();
        }

        public void Save()
        {
            if (!Directory.Exists(CSMD5Path))
                Directory.CreateDirectory(CSMD5Path);

            GUID = AssetDatabase.AssetPathToGUID(FilePath);
            MD5 = UtilTool.GetMD5String(StringBuilder.ToString());
            NeedReImport = false;

            if (string.IsNullOrEmpty(GUID) || !File.Exists(CSMD5Path + GUID) ||
                !File.Exists(FilePath) || File.ReadAllText(CSMD5Path + GUID) != MD5)
            {
                NeedReImport = true;
                UtilTool.WriteAllText(FilePath, StringBuilder.ToString());
            }

            if (NeedReImport)
                AssetDatabase.ImportAsset(FilePath);

            GUID = AssetDatabase.AssetPathToGUID(FilePath);
            UtilTool.WriteAllText(CSMD5Path + GUID, MD5);
        }
    }
}
