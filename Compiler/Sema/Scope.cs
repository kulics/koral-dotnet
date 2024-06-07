using Compiler.AstNodes;
using Compiler.Types;
using System.Collections.Generic;

namespace Compiler.Sema
{
    internal class Scope()
    {
        readonly Dictionary<string, Identifier> identifiers = [];
        readonly Dictionary<string, KoralType> types = [];
        public bool IsLoop { get; init; }

        public void PushId(Identifier id) => identifiers[id.Name] = id;
        public Identifier? GetId(string id)
        {
            if (identifiers.TryGetValue(id, out var result))
            {
                return result;
            }
            return null;
        }
        public void PushType(KoralType typeInfo) => types[typeInfo.Name] = typeInfo;
        public KoralType? GetTypeInfo(string typeInfo)
        {
            if (types.TryGetValue(typeInfo, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
