"Compiler" {
    "Antlr4/Runtime"
    "Antlr4/Runtime/Misc"
    "System"

    KParser. = "Compiler" 
    Compiler_static. = "Compiler" 
}

KLangVisitor : $ (me) {
    .VisitJudgeCaseStatement : (context JudgeCaseStatementContext -> v any)  {
        obj = ""
        expr = Visit(context.expression()) Result!
        obj += "switch (\{expr.text}) \{BlockLeft, Wrap}"
        @ item = context.caseStatement().. {
            r = Visit(item) str!
            obj += r + Wrap
        }
        obj += BlockRight+Wrap
        <- obj
    }

    .VisitCaseExprStatement : (context CaseExprStatementContext -> v any) {
        <- ? -> context.expression() >< nil  {
            expr = Visit(context.expression()) Result!
            "case \{expr.text} :\{Wrap}"
        } context.typeType() >< nil {
            id = "it"
            ? context.id() >< nil {
                id = (Visit(context.id()) Result!).text
            }
            me.add_id(id)
            type = Visit(context.typeType()) str!
            "case \{type} \{id} :\{Wrap}"
        } _ {
            "default:\{Wrap}"
        }
    }

    .VisitCaseStatement : (context CaseStatementContext -> v any) {
        obj = ""
        @ item = context.caseExprStatement().. {
            r = Visit(item) str!
            me.add_current_set()
            process = "\{BlockLeft} \{ProcessFunctionSupport(context.functionSupportStatement()), BlockRight} break;"
            me.delete_current_set()
            obj += r + process
        }
        <- obj
    }

    .VisitJudgeStatement : (context JudgeStatementContext -> v any) {
        obj = ""
        obj += Visit(context.judgeIfStatement())
        @ it = context.judgeElseIfStatement().. {
            obj += Visit(it)
        }
        ? context.judgeElseStatement() >< nil {
            obj += Visit(context.judgeElseStatement())
        }
        <- obj
    }

    .VisitJudgeIfStatement : (context JudgeIfStatementContext -> v any) {
        b = Visit(context.expression()) Result!
        obj = "if ( \{b.text} ) \{BlockLeft, Wrap}"
        me.add_current_set()
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        me.delete_current_set()
        obj += BlockRight+Wrap
        <- obj
    }

    .VisitJudgeElseIfStatement : (context JudgeElseIfStatementContext -> v any) {
        b = Visit(context.expression()) Result!
        obj = "else if ( \{b.text} ) \{BlockLeft, Wrap}"
        me.add_current_set()
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        me.delete_current_set()
        obj += BlockRight+Wrap
        <- obj
    }

    .VisitJudgeElseStatement : (context JudgeElseStatementContext -> v any) {
        obj = "else \{BlockLeft, Wrap}"
        me.add_current_set()
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        me.delete_current_set()
        obj += BlockRight+Wrap
        <- obj
    }

    .VisitJudgeExpression : (context JudgeExpressionContext -> v any) {
        r = Result{}
        r.data = "var"
        r.text = "run(()=>" + BlockLeft
        r.text += Visit(context.judgeExpressionIfStatement())
        @ it = context.judgeExpressionElseIfStatement().. {
            r.text += Visit(it)
        }
        ? context.judgeExpressionElseStatement() >< nil {
            r.text += Visit(context.judgeExpressionElseStatement())
        }
        r.text += BlockRight + ")"
        <- r
    }

    .VisitJudgeExpressionIfStatement : (context JudgeExpressionIfStatementContext -> v any) {
        b = Visit(context.expression()) Result!
        obj = "if ( \{b.text} ) \{BlockLeft, Wrap}"
        me.add_current_set()
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        obj += "return \{ (Visit(context.tupleExpression()) Result!).text };"
        me.delete_current_set()
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitJudgeExpressionElseIfStatement : (context JudgeExpressionElseIfStatementContext -> v any) {
        b = Visit(context.expression()) Result!
        obj = "else if ( \{b.text} ) \{BlockLeft, Wrap}"
        me.add_current_set()
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        obj += "return \{ (Visit(context.tupleExpression()) Result!).text };"
        me.delete_current_set()
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitJudgeExpressionElseStatement : (context JudgeExpressionElseStatementContext -> v any) {
        obj = "else \{BlockLeft, Wrap}"
        me.add_current_set()
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        obj += "return \{ (Visit(context.tupleExpression()) Result!).text };"
        me.delete_current_set()
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitJudgeCaseExpression : (context JudgeCaseExpressionContext -> v any)  {
        r = Result{}
        expr = Visit(context.expression()) Result!
        r.text = "run(()=> { switch (\{expr.text}) "
        r.text += BlockLeft+Wrap
        @ item = context.caseExpressionStatement().. {
            temp = Visit(item) str!
            r.text += temp + Wrap
        }
        r.text += BlockRight + Wrap + "})"
        <- r
    }

    .VisitCaseExpressionStatement : (context CaseExpressionStatementContext -> v any) {
        obj = ""
        @ item = context.caseExprStatement().. {
            r = Visit(item) str!
            me.add_current_set()
            process = BlockLeft + ProcessFunctionSupport(context.functionSupportStatement())
            process += "return \{ (Visit(context.tupleExpression()) Result!).text };"
            process += "\{ BlockRight }break;"
            me.delete_current_set()
            obj += r + process
        }
        <- obj
    }
}
