using System.Text;

namespace SAT.Sanitizer
{
    public class CSanitizer
    {
        public static string Sanitize(string val)
        {
            StringBuilder ret = new();
            char streak = 'a';

            foreach (char c in val)
            {
                char low = char.ToLower(c);

                if (char.IsLetter(char.ToLower(c)))
                {
                    if (c < streak)
                    {
                        ret.Append(c);
                    }
                    else
                    {
                        ret.Append(streak);
                        streak = (char)((byte)streak + 1);
                    }
                } 
                else if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else
                {
                    ret.Append(low);
                }
            }

            return ret.ToString();
        }
    }
}
