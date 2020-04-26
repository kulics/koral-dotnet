Compiler . {
    Antlr4.Runtime
    Antlr4.Runtime.Misc
    System
    System.Collections.Generic

    _ : Compiler.KParser
    _ : Compiler.Compiler_static
}


KLangVisitor : $ me {
    .VisitImplementStatement : (context ImplementStatementContext -> v any) {
        id : Visit(context.id()) Result!
        
        obj! : ""
        extend! : []str{}
        @ item : context.includeStatement() {
            r : Visit(item) str!
            extend += r
        }
        @ item : context.packageFieldStatement() {
            r : Visit(item) Result!
            obj += r.text
        }
        @ item : context.packageNewStatement() {
            r : Visit(item) str!
            obj += "public ${id.text} ${r}"
        }
        obj += BlockRight + Wrap
        header! : ""
        header += "${id.permission} partial class ${id.text}"
        # 泛型
        template! : ""
        templateContract! : ""
        ? context.templateDefine() >< nil {
            item : Visit(context.templateDefine()) TemplateItem!
            template += item.Template
            templateContract = item.Contract
            header += template
        }

        ? extend.length > 0 {
            temp : extend[0]
            @ i : 1 ~ extend.length-1 {
                temp += "," + extend[i]
            }
            header += ":" + temp
        }

        header += templateContract + BlockLeft + Wrap
        obj = header + obj
        me.selfID = ""
        me.superID = ""
        <- obj
    }

    .VisitOverrideVariableStatement : (context OverrideVariableStatementContext -> v any) {
        r1 : Visit(context.id()) Result!
        isMutable! : r1.isVirtual
        isVirtual! : " override "
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
                pri = "private ${typ} _${r1.text}" 
                ? r2 >< nil {
                    pri += " = " + r2.text
                }
                pri += Terminate + Wrap
            }
            obj = pri + obj
            obj += "${r1.permission} ${isVirtual} ${typ} ${r1.text; BlockLeft}"
            @ v : me.selfPropertyContent {
                obj += v
            }
            obj += BlockRight+Wrap
            me.selfPropertyContent.clear()
            me.selfPropertyID = ""
            me.selfPropertyVariable = false
        } _ {
            obj += "${r1.permission} ${typ} ${r1.text}"
            obj += ? -> r2 >< nil {
                " = ${r2.text; Terminate; Wrap}"
            } _ {
                Terminate + Wrap
            }
        }
        <- obj
    }

    .VisitOverrideFunctionStatement : (context OverrideFunctionStatementContext -> v any) {
        id : Visit(context.id()) Result!
        isVirtual! : " override "
        obj! : ""
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
            obj += "${isVirtual} ${pout} ${id.text}"
        } _ {
            ? context.y >< nil {
                ? pout >< "void" {
                    pout = "${IEnum}<${pout}>"
                }
            }
            obj += "${isVirtual} ${pout} ${id.text}"
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
        ? context.n >< nil {
            obj = "protected " + obj
        } _ {
            obj = "${id.permission} " + obj
        }
        <- obj
    }
}
