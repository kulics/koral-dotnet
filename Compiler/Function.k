Compiler . {
    Antlr4.Runtime
    Antlr4.Runtime.Misc
    System
    System.Collections.Generic

    _ : Compiler.KParser
    _ : Compiler.Compiler_static
}


Parameter : $ {
    id! str
    type! str
    value! str
    annotation! str
    permission! str
}

KLangVisitor : $ me {
    ProcessFunctionSupport : (items [,]FunctionSupportStatementContext -> v str) {
        obj! : ""
        content! : ""
        lazy! : []str{}
        @ item : items {
            content += ? -> item.GetChild(0) UsingStatementContext? {
                lazy.add("}")
                "using (${Visit(item) str!}) ${BlockLeft; Wrap}"
            } _ {
                Visit(item)
            }
        }
        ? lazy.Count > 0 {
            @ i : lazy.Count-1 ... 0 {
                content += BlockRight
            }
        }
        obj += content
        <- obj
    }

    .VisitFunctionStatement : (context FunctionStatementContext -> v any) {
        id! : Visit(context.id()) Result!
        obj! : ""
        # "异步"
        pout! : ""
        ? context.parameterClauseOut() >< nil {
            pout = Visit(context.parameterClauseOut()) str!
        }
        ? context.t.Type == Right_Flow {
            pout = ? -> pout >< "void" {
                "${Task}<${pout}>"
            } _ {
                Task
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
        # "泛型"
        templateContract! : ""
        ? context.templateDefine() >< nil {
            template : Visit(context.templateDefine()) TemplateItem!
            obj += template.Template
            templateContract = template.Contract
        }
        me.add_current_set()
        me.add_func_stack()
        obj += "${Visit(context.parameterClauseIn())} ${templateContract; Wrap; BlockLeft; Wrap} "
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        obj += BlockRight + Wrap
        me.delete_current_set()
        # "异步"
        ? get_func_async() {
            obj = " async " + obj
        }
        me.delete_func_stack()
        <- obj
    }

    .VisitReturnStatement : (context ReturnStatementContext -> v any) {
        ? context.tupleExpression() >< nil {
            r : Visit(context.tupleExpression()) Result!
            <- "return ${r.text; Terminate; Wrap}"
        }
        <- "return${Terminate; Wrap}"
    }

    .VisitReturnAsyncStatement : (context ReturnAsyncStatementContext -> v any) {
        ? context.tupleExpression() >< nil {
            r : Visit(context.tupleExpression()) Result!
            <- "return Task.FromResult(${r.text})${Terminate; Wrap}"
        }
        <- "return Task.FromResult(true)${Terminate; Wrap}"
    }

    .VisitYieldReturnStatement : (context YieldReturnStatementContext -> v any) {
        r : Visit(context.tupleExpression()) Result!
        <- "yield return ${r.text; Terminate; Wrap}"
    }

    .VisitYieldBreakStatement : (context YieldBreakStatementContext -> v any) {
        <- "yield break${Terminate; Wrap}"
    }

    .VisitTuple : (context TupleContext -> v any) {
        obj! : "("
        @ i : 0 .. context.expression().Length-1 {
            r : Visit(context.expression(i)) Result!
            obj += ? -> i == 0 {
                r.text
            } _ {
                ", " + r.text
            }
        }
        obj += ")"
        <- Result{
            data = "var"
            text = obj
        }
    }

    .VisitTupleExpression : (context TupleExpressionContext -> v any) {
        obj! : ""
        @ i : 0 .. context.expression().Length-1 {
            r : Visit(context.expression(i)) Result!
            obj += ? -> i == 0 {
                r.text
            } _ {
                ", " + r.text
            }
        }
        ? context.expression().Length > 1 {
            obj = "(${obj})"
        }
        <- Result{
            data = "var"
            text = obj
        }
    }

    .VisitParameterClauseIn : (context ParameterClauseInContext -> v any) {
        obj! : "("
        temp! : []str{}
        @ i : context.parameter().Length-1 ... 0 {
            p : Visit(context.parameter(i)) Parameter!
            temp.add("${p.annotation} ${p.type} ${p.id} ${p.value}")
            me.add_id(p.id)
        }
        @ i : temp.Count-1 ... 0 {
            obj += ? -> i == temp.Count - 1 {
                temp[i]
            } _ {
                ", " + temp[i]
            }
        }

        obj += ")"
        <- obj
    }

    .VisitParameterClauseOut : (context ParameterClauseOutContext -> v any) {
        obj! : ""
        ? context.parameter().Length == 0 {
            obj += "void"
        } context.parameter().Length == 1 {
            p : Visit(context.parameter(0)) Parameter!
            obj += p.type
        }
        ? context.parameter().Length > 1 {
            obj += "( "
            temp! : []str{}
            @ i : context.parameter().Length-1 ... 0 {
                p : Visit(context.parameter(i)) Parameter!
                temp.add("${p.annotation} ${p.type} ${p.id} ${p.value}")
            }
            @ i : temp.Count-1 ... 0 {
                obj += ? -> i == temp.Count - 1 {
                    temp[i]
                } _ {
                    ", " + temp[i]
                }
            }
            obj += " )"
        }
        <- obj
    }

    .VisitParameter : (context ParameterContext -> v any) {
        p : Parameter{}
        id : Visit(context.id()) Result!
        p.id = id.text
        p.permission = id.permission
        ? context.annotationSupport() >< nil {
            p.annotation = Visit(context.annotationSupport()) str!
        }
        ? context.expression() >< nil {
            p.value = "= ${(Visit(context.expression()) Result!).text}"
        }
        p.type = Visit(context.typeType()) str!
        ? context.Comma_Comma_Comma() >< nil {
            p.type = "params ${p.type}[]"
        }
        ? context.Bang() >< nil {
            p.type = "ref ${p.type}"
        }
        <- p
    }
}
