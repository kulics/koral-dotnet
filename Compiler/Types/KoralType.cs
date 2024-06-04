using Compiler.Library;
using Compiler.AstNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Compiler.Types
{
    public abstract class KoralType
    {
        public string Name { get; }
        public string UniqueName { get; }

        internal KoralType(string name, string uniqueName)
        {
            Name = name;
            UniqueName = uniqueName;
        }

        public bool TypeEquals(KoralType rhs)
        {
            if (ReferenceEquals(this, rhs))
            {
                return true;
            }
            else if (UniqueName == rhs.UniqueName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class RecordType(
        string name,
        Dictionary<string, Identifier> members,
        string uniqueName,
        (string, List<KoralType>)? rawGenericsType = null
    ) : KoralType(name, uniqueName)
    {
        public Dictionary<string, Identifier> Members { get; } = members;

        public (string, List<KoralType>)? RawGenericsType = rawGenericsType;
    }

    public class FunctionType(List<KoralType> paramTypes, KoralType returnType) : KoralType(
        MakeFunctionTypeName(paramTypes, returnType),
        MakeGenericsUniqueName("Func", paramTypes, returnType))
    {
        public List<KoralType> ParamTypes { get; } = paramTypes;
        public KoralType ReturnType { get; } = returnType;

        static string MakeFunctionTypeName(List<KoralType> paramTypes, KoralType returnType)
        {
            var sb = new StringBuilder();
            sb.Append("fn(");
            foreach (var (index, paramType) in paramTypes.WithIndex())
            {
                if (index != 0)
                {
                    sb.Append(',');
                }
                sb.Append(paramType.Name);
            }
            sb.Append(')');
            sb.Append(returnType.Name);
            return sb.ToString();
        }

        static string MakeGenericsUniqueName(string name, List<KoralType> typeArgs, KoralType returnType)
        {
            var sb = new StringBuilder();
            sb.Append(name);
            sb.Append("_OP");
            foreach (var item in typeArgs)
            {
                sb.Append('_');
                sb.Append(item.UniqueName);
            }
            sb.Append('_');
            sb.Append(returnType.UniqueName);
            sb.Append("_ED");
            return sb.ToString();
        }
    }

    public sealed class TypeParameter(
        string name,
        ConstraintType constraint,
        bool isFromExtension = false
    ) : KoralType(name, $"For_All_{name}")
    { }

    public abstract class ConstraintType { }
}
