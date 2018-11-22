using System.Text;

namespace IceCake.Core
{
    public static class StringBuilderExtension
    {
        public static StringBuilder Format(this StringBuilder self, string format, params object[] _params)
        {
            return self.AppendFormat(format, _params);
        }

        public static StringBuilder Tab(this StringBuilder self, int count)
        {
            for (int index = 0; index < count; index++)
            {
                self.Append('\t');
            }
            return self;
        }

        public static StringBuilder Space(this StringBuilder self, int count)
        {
            for (int index = 0; index < count; index++)
            {
                self.Append(' ');
            }
            return self;
        }

        public static StringBuilder Lines(this StringBuilder self, int count)
        {
            for (int index = 0; index < count; index++)
            {
                self.Line();
            }
            return self;
        }

        public static StringBuilder Line(this StringBuilder self)
        {
            return self.AppendLine();
        }
    }
}
