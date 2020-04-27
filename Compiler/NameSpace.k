Compiler . {
    Antlr4.Runtime
    Antlr4.Runtime.Misc
    System
    System.Collections.Generic

    _ : Compiler.KParser
    _ : Compiler.Compiler_static
}

Namespace : $ {
    name! str
    imports! str
}

KLangVisitor : $ me {
    .VisitStatement : (context StatementContext -> v any) {
        obj! : ""
        ns : Visit(context.exportStatement()) Namespace!
        # import library
        obj += "using Library;${Wrap}using static Library.Lib;${Wrap}"
        obj += ns.imports + Wrap
        ? context.annotationSupport() >< nil {
            obj += Visit(context.annotationSupport())
        }
        obj += "namespace ${ns.name; Wrap; BlockLeft; Wrap}"

        content! : ""
        contentStatic! : ""
        me.add_current_set()
        # pre process to get type id
        @ item : context.namespaceSupportStatement() {
            child : item.GetChild(0)
            type : child.GetType()
            ? type == typeof(PackageStatementContext) {
                childContext : child PackageStatementContext!
                id : Visit(childContext.id()) Result!
                me.add_type(id.text)
            } type == typeof(TypeTagStatementContext) {
                childContext : child TypeTagStatementContext!
                me.add_type(childContext.Comment_Tag().GetText().sub_str(2))
            }
        }
        @ item : context.namespaceSupportStatement() {
            type : item.GetChild(0).GetType()
            ? type == typeof(NamespaceVariableStatementContext) |
                type == typeof(NamespaceFunctionStatementContext) |
                type == typeof(NamespaceConstantStatementContext) {
                contentStatic += Visit(item)
            } _ {
                content += Visit(item)
            }
        }
        obj += content
        ? contentStatic >< "" {
            obj += "public partial class ${ns.name.sub_str(ns.name.last_index_of(".") + 1)}_static ${BlockLeft; Wrap; contentStatic; BlockRight; Wrap}"
        }
        me.delete_current_set()
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitExportStatement : (context ExportStatementContext -> v any) {
        name : Visit(context.nameSpaceItem()) str!
        obj : Namespace{
            name = name
        }
        @ item : context.importStatement() {
            obj.imports += Visit(item) str!
        }
        <- obj
    }

    .VisitImportStatement : (context ImportStatementContext -> v any) {
        obj! : ""
        ? context.annotationSupport() >< nil {
            obj += Visit(context.annotationSupport())
        }
        ns : Visit(context.nameSpaceItem()) str!
        obj += ? -> context.Discard() >< nil {
            "using static ${ ns }"
        } context.id() >< nil {
            "using ${ns}.${(Visit(context.id()) Result!).text}"
        } _ {
            "using ${ns}"
        }
        obj += Terminate + Wrap
        <- obj
    }

    .VisitNameSpaceItem : (context NameSpaceItemContext -> v any) {
        obj! : ""
        @ i : 0 .. context.id().Length-1 {
            id : Visit(context.id(i)) Result!
            obj += ? -> i == 0 {
                id.text
            } _ {
                "." + id.text
            }
        }
        <- obj
    }

    .VisitName : (context NameContext -> v any) {
        obj! : ""
        @ i : 0 .. context.id().Length-1 {
            id : Visit(context.id(i)) Result!
            obj += ? -> i == 0 {
                id.text
            } _ {
                "." + id.text
            }
        }
        <- obj
    }

    .VisitEnumStatement : (context EnumStatementContext -> v any) {
        obj! : ""
        id : Visit(context.id()) Result!
        header! : ""
        # typ = Visit(context.typeType()) str!
        typ! : "int"
        ? context.annotationSupport() >< nil {
            header += Visit(context.annotationSupport())
        }
        header += "${id.permission} enum ${id.text}:${typ}"
        header += Wrap + BlockLeft + Wrap
        @ i : 0 .. context.enumSupportStatement().Length-1 {
            obj += Visit(context.enumSupportStatement(i))
        }
        obj += BlockRight + Terminate + Wrap
        obj = header + obj
        <- obj
    }

    .VisitEnumSupportStatement : (context EnumSupportStatementContext -> v any) {
        id : Visit(context.id()) Result!
        ? context.integerExpr() >< nil {
            op! : ""
            ? context.add() >< nil {
                op = Visit(context.add()) str!
            }
            id.text += " = " + op + Visit(context.integerExpr())
        }
        <- id.text + ","
    }

    .VisitNamespaceFunctionStatement : (context NamespaceFunctionStatementContext -> v any) {
        id : Visit(context.id()) Result!
        obj! : ""
        ? context.annotationSupport() >< nil {
            obj += Visit(context.annotationSupport())
        }
        # 异步
        pout! : ""
        ? context.parameterClauseOut() >< nil {
            pout = Visit(context.parameterClauseOut()) str!
        }
        ? context.t.Type == Right_Flow {
            ? pout >< "void" {
                ? context.y >< nil {
                    pout = "${IEnum}<${pout}>"
                }
                pout = "${Task}<${pout}>"
            } _ {
                pout = Task
            }
            obj += "${pout} ${id.text}"
        } _ {
            ? context.y >< nil {
                ? pout >< "void" {
                    pout = "${IEnum}<${pout}>"
                }
            }
            obj += "${pout} ${id.text}"
        }

        # 泛型
        templateContract! : ""
        ? context.templateDefine() >< nil {
            template : Visit(context.templateDefine()) TemplateItem!
            obj += template.Template
            templateContract = template.Contract
        }
        me.add_current_set()
        me.add_func_stack()
        obj += Visit(context.parameterClauseIn()) + templateContract + BlockLeft + Wrap
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        me.delete_current_set()
        obj += BlockRight + Wrap
        # 异步
        ? get_func_async() {
            obj = " async " + obj
        }
        me.delete_func_stack()
        obj = id.permission + " static " + obj
        <- obj
    }

    .VisitNamespaceConstantStatement : (context NamespaceConstantStatementContext -> v any) {
        id : Visit(context.id()) Result!
        expr : Visit(context.expression()) Result!
        typ! : ? -> context.typeType() >< nil {
            Visit(context.typeType()) str!
        } _ {
            expr.data str!
        }

        obj! : ""
        ? context.annotationSupport() >< nil {
            obj += Visit(context.annotationSupport())
        }
        
        obj += "${id.permission} const ${typ} ${id.text} = ${expr.text; Terminate; Wrap}"
        <- obj
    }

    .VisitNamespaceVariableStatement : (context NamespaceVariableStatementContext -> v any) {
        r1 : Visit(context.id()) Result!
        me.add_id(r1.text)
        isMutable! : r1.isVirtual
        typ! : ""
        r2! Result : nil
        ? context.expression() >< nil {
            r2 = Visit(context.expression()) Result!
            typ = r2.data str!
        }
        ? context.typeType() >< nil {
            typ = Visit(context.typeType()) str!
        }
        obj! : ""
        ? context.annotationSupport() >< nil {
            me.selfPropertyID = r1.text
            obj += Visit(context.annotationSupport())
        }
        ? me.selfPropertyContent.len > 0 {
            pri! : ""
            ? me.selfPropertyVariable {
                pri = "private static ${typ} _${r1.text}" 
                ? r2 >< nil {
                    pri += " = " + r2.text
                }
                pri += Terminate + Wrap
            }
            obj = pri + obj
            obj += "${r1.permission} static ${typ} ${r1.text; BlockLeft}"
            @ v : me.selfPropertyContent {
                obj += v
            }
            obj += BlockRight+Wrap
            me.selfPropertyContent.clear()
            me.selfPropertyID = ""
            me.selfPropertyVariable = false
        } _ {
            obj += "${r1.permission} static ${typ} ${r1.text}"
            obj += ? -> r2 >< nil {
                " = ${r2.text; Terminate; Wrap}"
            } _ {
                Terminate + Wrap
            }
        }
        <- obj
    }

    .VisitTypeTagStatement : (context TypeTagStatementContext -> v any) {
        <- ""
    }
}
