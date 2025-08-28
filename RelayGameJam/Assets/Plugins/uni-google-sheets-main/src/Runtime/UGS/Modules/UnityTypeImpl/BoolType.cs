using UnityEngine;

namespace GoogleSheet.Type
{
    [Type(Type: typeof(int), TypeName: new string[] { "bool", "Bool", "BOOL" })]
    public class BoolType : IType
    {

        public object DefaultValue => false;
        public object Read(string value)
        {
            if(value == "true" || value == "True" || value == "TRUE")
                return true;
            else if( value == "false" || value == "False" || value == "FALSE")
                return false;
            else throw new UGSValueParseException("bool값이 아님");
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}