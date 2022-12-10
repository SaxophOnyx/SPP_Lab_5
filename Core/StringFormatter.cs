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

            int pos = 0;
            int? paramNameStartIndex = null;
            int balance = 0;
            while (pos < template.Length)
            {
                char chr = template[pos];

                switch (chr)
                {
                    case '{':
                    {
                        ++balance;
                        if (balance == 2)
                        {
                            resBuilder.Append('{');
                            balance = 0;
                        }

                        break;
                    }

                    case '}':
                    {
                        --balance;
                        if (balance == -2)
                        {
                            resBuilder.Append('}');
                            balance = 0;
                        }
                        else if (balance == 0)
                        {
                            string paramName = template.Substring((int)paramNameStartIndex, pos - (int)paramNameStartIndex);
                            paramNameStartIndex = null;
                            var obj = accessorCash.AddOrUse(target, paramName);
                            resBuilder.Append(obj.ToString());
                        }

                        break;
                    }

                    default:
                    {
                        if (paramNameStartIndex == null)
                        {
                            if (balance == 0)
                                resBuilder.Append(chr);
                            else
                                paramNameStartIndex = pos;
                        }

                        break;
                    }
                }

                ++pos;
            }

            if (balance != 0)
                throw new InvalidFormatException();

            return resBuilder.ToString();
        }
    }
}
