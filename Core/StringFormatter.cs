using Core.Exceptions;
using System.Text;

namespace Core
{
    public partial class StringFormatter : IStringFormatter
    {
        public static StringFormatter Shared
        {
            get { return _shared; }
        }

        private static readonly StringFormatter _shared = new();
        private static readonly AccessorCash accessorCash = new();


        public StringFormatter()
        {

        }


        public string Format(string template, object target)
        {
            StringBuilder resBuilder = new();
            int balance = 0;
            int pos = 0;
            int? nameStartPos = null;
            char? prevChr = null;

            while (pos < template.Length)
            {
                char chr = template[pos];

                switch (chr)
                {
                    case '{':
                    {
                        if ((prevChr == '{') && (balance > 0))
                        {
                            balance = 0;
                            resBuilder.Append('{');
                        }
                        else
                        {
                            ++balance;
                        }

                        break;
                    }

                    case '}':
                    {
                        if (prevChr == '{')
                            throw new InvalidFormatException();

                        --balance;

                        if (balance == -2)
                        {
                            if (prevChr == '}')
                            {
                                balance = 0;
                                resBuilder.Append('}');
                            }
                            else
                            {
                                throw new InvalidFormatException();
                            }
                        }
                        else
                        {
                            if (balance == 0)
                            {
                                string paramName = template.Substring((int)nameStartPos, pos - (int)nameStartPos);
                                nameStartPos = null;
                                var obj = accessorCash.AddOrUse(target, paramName);
                                resBuilder.Append(obj.ToString());
                            }
                        }

                        break;
                    }

                    default:
                    {
                        if (nameStartPos == null)
                        {
                            if (balance == 0)
                                resBuilder.Append(chr);
                            else
                                nameStartPos = pos;
                        }

                        break;
                    }
                }

                prevChr = chr;
                ++pos;
            }

            if (balance != 0)
                throw new InvalidFormatException();

            return resBuilder.ToString();
        }
    }
}
