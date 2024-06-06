using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Compiler.Types;

namespace Compiler.AstNodes
{
    public abstract record class DeclarationNode : Node
    {
        internal DeclarationNode() { }
    }

    public sealed record class ModuleDeclarationNode(string Name) : DeclarationNode
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class GlobalFunctionDeclarationNode(
            Identifier Id,
            List<TypeParameter> TypeParameter,
            List<ParameterDeclarationNode> ParameterTypes,
            KoralType ReturnType,
            ExpressionNode Body) : DeclarationNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class ParameterDeclarationNode(Identifier Id, KoralType ParamType) : Node()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class GlobalVariableDeclarationNode(Identifier Id, ExpressionNode InitValue) : DeclarationNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class GlobalRecordDeclarationNode(
        KoralType Type,
        List<TypeParameter> TypeParameter,
        List<Identifier> Fields,
        List<MethodNode> Methods,
        KoralType? Implements
    ) : DeclarationNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class GlobalInterfaceDeclarationNode(
        KoralType Type,
        List<TypeParameter> TypeParameter,
        List<VirtualMethodNode> Methods
    ) : DeclarationNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public abstract record class MemberNode : Node
    {
        internal MemberNode() { }
    }

    public sealed record class MethodNode(
        Identifier Id,
        List<Identifier> Parameters,
        KoralType ReturnType,
        ExpressionNode Body,
        bool IsOverride
    ) : MemberNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
    public sealed record class VirtualMethodNode(
            Identifier Id,
            List<Identifier> Parameters,
            KoralType ReturnType
        ) : MemberNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
}
