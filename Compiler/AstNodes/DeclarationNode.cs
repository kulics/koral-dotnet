using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Compiler.Types;

namespace Compiler.AstNodes
{
    public abstract class DeclarationNode : Node
    {
        internal DeclarationNode() { }
    }

    public sealed class ModuleDeclarationNode(string name) : DeclarationNode
    {
        public string Name { get; set; } = name;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class GlobalFunctionDeclarationNode(
            Identifier id,
            List<TypeParameter> typeParameter,
            List<ParameterDeclarationNode> parameterTypes,
            KoralType returnType,
            ExpressionNode body) : DeclarationNode()
    {
        public Identifier Id { get; } = id;
        public List<TypeParameter> TypeParameter = typeParameter;
        public List<ParameterDeclarationNode> Parameters { get; } = parameterTypes;
        public KoralType returnType { get; } = returnType;
        public ExpressionNode Body { get; } = body;

        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class ParameterDeclarationNode(Identifier id, KoralType paramType) : Node()
    {
        public Identifier Id { get; } = id;
        public KoralType ParameterType { get; } = paramType;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class GlobalVariableDeclarationNode(Identifier id, ExpressionNode initValue) : DeclarationNode()
    {
        public Identifier Id { get; } = id;
        public ExpressionNode Expression { get; } = initValue;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class GlobalRecordDeclarationNode(
        KoralType type,
        List<TypeParameter> typeParameter,
        List<Identifier> fields,
        List<MethodNode> methods,
        KoralType? implements
    ) : DeclarationNode()
    {
        public KoralType Type { get; } = type;
        public List<TypeParameter> typeParameters { get; } = typeParameter;
        public List<Identifier> Fields { get; } = fields;
        public List<MethodNode> Methods { get; } = methods;
        public KoralType? Implements { get; } = implements;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class GlobalInterfaceDeclarationNode(
        KoralType type,
        List<TypeParameter> typeParameter,
        List<VirtualMethodNode> methods
    ) : DeclarationNode()
    {
        public KoralType Type { get; } = type;
        public List<TypeParameter> TypeParameters = typeParameter;
        public List<VirtualMethodNode>  Methods = methods;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public abstract class MemberNode : Node
    {
        internal MemberNode() { }
    }

    public sealed class MethodNode(
        Identifier id,
        List<Identifier> parameters,
        Type returnType,
        ExpressionNode body,
        bool isOverride
    ) : MemberNode()
    {
        public Identifier Id { get; } = id;
        public List<Identifier> Parameters { get; } = parameters;
        public Type ReturnType { get; } = returnType;
        public ExpressionNode Body { get; } = body;
        public bool IsOverride { get; } = isOverride;
    }
    public sealed class VirtualMethodNode(
            Identifier id,
            List<Identifier> parameters,
            KoralType returnType
        ) : MemberNode()
    {
        public Identifier Id { get; } = id;
        public List<Identifier> Parameters { get; } = parameters;
        public KoralType ReturnType { get; } = returnType;
    }
}
