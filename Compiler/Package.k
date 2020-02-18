Compiler . {
    Antlr4.Runtime
    Antlr4.Runtime.Misc
    System
    System.Collections.Generic

    _ = Compiler.KParser
    _ = Compiler.Compiler_static
}


KLangVisitor : $ (me) {
    .VisitIncludeStatement : (context IncludeStatementContext -> v any) {
        <- Visit(context.typeType())
    }

    .VisitPackageStatement : (context PackageStatementContext -> v any) {
        id = Visit(context.id()) Result!

        obj = ""
        extend = []str{}
        @ item = context.includeStatement().. {
            r = Visit(item) str!
            extend += r
        }
        @ item = context.packageStaticStatement().. {
            r = Visit(item) Result!
            obj += r.text
        }
        @ item = context.packageFieldStatement().. {
            r = Visit(item) Result!
            obj += r.text
        }
        @ item = context.packageImplementStatement().. {
            r = Visit(item) Result!
            extend += r.data str!
            obj += r.text
        }
        @ item = context.packageNewStatement().. {
            r = Visit(item) str!
            obj += "public \{id.text} \{r}"
        }
        obj += BlockRight + Wrap
        header = ""
        ? context.annotationSupport() >< nil {
            header += Visit(context.annotationSupport())
        }
        header += "\{id.permission} partial class \{id.text}"
        # 泛型
        template = ""
        templateContract = ""
        ? context.templateDefine() >< nil {
            item = Visit(context.templateDefine()) TemplateItem!
            template += item.Template
            templateContract = item.Contract
            header += template
        }

        ? extend.length > 0 {
            temp = extend[0]
            @ i = 1 ~ extend.length-1.. {
                temp += "," + extend[i]
            }
            header += ":" + temp
        }

        header += templateContract + BlockLeft + Wrap
        obj = header + obj
        <- obj
    }

    .VisitPackageStaticStatement : (context PackageStaticStatementContext -> v any) {
        obj = ""
        @ item = context.packageStaticSupportStatement().. {
            obj += Visit(item)
        }
        <- Result{text = obj}
    }

    .VisitPackageStaticVariableStatement : (context PackageStaticVariableStatementContext -> v any) {
        r1 = Visit(context.id()) Result!
        typ = ""
        r2 Result = nil
        ? context.expression() >< nil {
            r2 = Visit(context.expression()) Result!
            typ = r2.data str!
        }
        ? context.typeType() >< nil {
            typ = Visit(context.typeType()) str!
        }
        obj = ""
        ? context.annotationSupport() >< nil {
            me.selfPropertyID = r1.text
            obj += Visit(context.annotationSupport())
        }
        ? me.selfPropertyContent.len > 0 {
            pri = ""
            ? me.selfPropertyVariable {
                pri = "private static \{typ} _\{r1.text}" 
                ? r2 >< nil {
                    pri += " = " + r2.text
                }
                pri += Terminate + Wrap
            }
            obj = pri + obj
            obj += "\{r1.permission} static \{typ} \{r1.text; BlockLeft}"
            @ v = me.selfPropertyContent.. {
                obj += v
            }
            obj += BlockRight+Wrap
            me.selfPropertyContent.clear()
            me.selfPropertyID = ""
            me.selfPropertyVariable = false
        } _ {
            obj += "\{r1.permission} static \{typ} \{r1.text}"
            obj += ? -> r2 >< nil {
                " = \{r2.text; Terminate; Wrap}"
            } _ {
                Terminate + Wrap
            }
        }
        <- obj
    }

    .VisitPackageStaticFunctionStatement : (context PackageStaticFunctionStatementContext -> v any) {
        id = Visit(context.id()) Result!
        obj = ""
        # 异步
        obj += "\{id.permission} static "
        pout = ""
        ? context.parameterClauseOut() >< nil {
            pout = Visit(context.parameterClauseOut()) str!
        }
        ? context.t.Type == Right_Flow {
            ? context.Discard() >< nil {
                pout = "void"
            } pout >< "void" {
                ? context.y >< nil {
                    pout = "\{IEnum}<\{pout}>"
                }
                pout = "\{Task}<\{pout}>"
            } _ {
                pout = Task
            }
            obj += "async \{pout} \{id.text}"
        } _ {
            ? context.y >< nil {
                ? pout >< "void" {
                    pout = "\{IEnum}<\{pout}>"
                }
            }
            obj += "\{pout} \{id.text}"
        }
        # 泛型
        templateContract = ""
        ? context.templateDefine() >< nil {
            template = Visit(context.templateDefine()) TemplateItem!
            obj += template.Template
            templateContract = template.Contract
        }
        me.add_current_set()
        obj += Visit(context.parameterClauseIn()) + templateContract + BlockLeft + Wrap
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        me.delete_current_set()
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitPackageFieldStatement : (context PackageFieldStatementContext -> v any) {
        obj = ""
        extend = ""
        ? context.id(0) >< nil {
            Self = Visit(context.id(0)) Result!
            me.selfID = Self.text
        }
        ? context.id(1) >< nil {
            Super = Visit(context.id(1)) Result!
            me.superID = Super.text
        }
        @ item = context.packageSupportStatement().. {
            obj += Visit(item)
        }
        me.selfID = ""
        me.superID = ""
        <- Result{text = obj; data = extend}
    }

    .VisitPackageVariableStatement : (context PackageVariableStatementContext -> v any) {
        r1 = Visit(context.id()) Result!
        isMutable = r1.isVirtual
        isVirtual = ""
        ? r1.isVirtual {
            isVirtual = " virtual "
        }
        typ = ""
        r2 Result = nil
        ? context.expression() >< nil {
            r2 = Visit(context.expression()) Result!
            typ = r2.data str!
        }
        ? context.typeType() >< nil {
            typ = Visit(context.typeType()) str!
        }
        obj = ""
        ? context.annotationSupport() >< nil {
            me.selfPropertyID = r1.text
            obj += Visit(context.annotationSupport())
        }
        ? me.selfPropertyContent.len > 0 {
            pri = ""
            ? me.selfPropertyVariable {
                pri = "private \{typ} _\{r1.text}" 
                ? r2 >< nil {
                    pri += " = " + r2.text
                }
                pri += Terminate + Wrap
            }
            obj = pri + obj
            obj += "\{r1.permission} \{isVirtual} \{typ} \{r1.text; BlockLeft}"
            @ v = me.selfPropertyContent.. {
                obj += v
            }
            obj += BlockRight+Wrap
            me.selfPropertyContent.clear()
            me.selfPropertyID = ""
            me.selfPropertyVariable = false
        } _ {
            obj += "\{r1.permission} \{typ} \{r1.text}"
            obj += ? -> r2 >< nil {
                " = \{r2.text; Terminate; Wrap}"
            } _ {
                Terminate + Wrap
            }
        }
        <- obj
    }

    .VisitPackageFunctionStatement : (context PackageFunctionStatementContext -> v any) {
        id = Visit(context.id()) Result!
        isVirtual = ""
        ? id.isVirtual {
            isVirtual = " virtual "
        }
        obj = ""
        # 异步
        obj += "\{id.permission} "
        pout = ""
        ? context.parameterClauseOut() >< nil {
            pout = Visit(context.parameterClauseOut()) str!
        }
        ? context.t.Type == Right_Flow {
            ? context.Discard() >< nil {
                pout = "void"
            } pout >< "void" {
                ? context.y >< nil {
                    pout = "\{IEnum}<\{pout}>"
                }
                pout = "\{Task}<\{pout}>"
            } _ {
                pout = Task
            }
            obj += "\{isVirtual} async \{pout} \{id.text}"
        } _ {
            ? context.y >< nil {
                ? pout >< "void" {
                    pout = "\{IEnum}<\{pout}>"
                }
            }
            obj += "\{isVirtual} \{pout} \{id.text}"
        }
        # 泛型
        templateContract = ""
        ? context.templateDefine() >< nil {
            template = Visit(context.templateDefine()) TemplateItem!
            obj += template.Template
            templateContract = template.Contract
        }
        me.add_current_set()
        obj += Visit(context.parameterClauseIn()) + templateContract + BlockLeft + Wrap
        obj += ProcessFunctionSupport(context.functionSupportStatement())
        me.delete_current_set()
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitPackageNewStatement : (context PackageNewStatementContext -> v any) {
        text = ""
        ? context.id(0) >< nil {
            Self = Visit(context.id(0)) Result!
            me.selfID = Self.text
        }
        ? context.id(1) >< nil {
            Super = Visit(context.id(1)) Result!
            me.superID = Super.text
        }
        # 获取构造数据
        me.add_current_set()
        text += Visit(context.parameterClauseIn()) str!
        ? context.expressionList() >< nil {
            text += ":base(\{(Visit(context.expressionList()) Result!).text})"
        }
        text += BlockLeft + ProcessFunctionSupport(context.functionSupportStatement()) + BlockRight + Wrap
        me.delete_current_set()
        me.selfID = ""
        me.superID = ""
        <- text
    }

    .VisitPackageEventStatement : (context PackageEventStatementContext -> v any) {
        obj = ""
        id = Visit(context.id()) Result!
        nameSpace = Visit(context.nameSpaceItem())
        obj += "public event \{nameSpace} \{id.text; Terminate; Wrap}"
        <- obj
    }

    .VisitProtocolStatement : (context ProtocolStatementContext -> v any) {
        id = Visit(context.id()) Result!
        obj = ""
        interfaceProtocol = ""
        ptclName = id.text
        ? context.annotationSupport() >< nil {
            obj += Visit(context.annotationSupport())
        }
        @ item = context.protocolSubStatement().. {
            r = Visit(item) str!
            interfaceProtocol += r
        }
        extend = []str{}
        @ item = context.includeStatement().. {
            r = Visit(item) str!
            extend += r
        }
        obj += "public partial interface \{ptclName}"
        ? extend.length > 0 {
            temp = extend[0]
            @ i = 1 ~ extend.length-1.. {
                temp += "," + extend[i]
            }
            obj += ":" + temp
        }
        # 泛型
        templateContract = ""
        ? context.templateDefine() >< nil {
            template = Visit(context.templateDefine()) TemplateItem!
            obj += template.Template
            templateContract = template.Contract
        }
        obj += templateContract + BlockLeft + Wrap
        obj += interfaceProtocol
        obj += BlockRight + Wrap
        <- obj
    }

    .VisitProtocolSubStatement : (context ProtocolSubStatementContext -> v any) {
        obj = ""
        @ item = context.protocolSupportStatement().. {
            r = Visit(item) Result!
            obj += r.text
        }
        <- obj
    }

    .VisitProtocolVariableStatement : (context ProtocolVariableStatementContext -> v any) {
        id = Visit(context.id()) Result!
        isMutable = id.isVirtual
        r = Result{}
        ? context.annotationSupport() >< nil {
            r.text += Visit(context.annotationSupport())
        }
        r.permission = "public"
        type = Visit(context.typeType()) str!
        r.text += type + " " + id.text
        r.text += BlockLeft
        r.text += "get;set;"
        r.text += BlockRight + Wrap
        <- r
    }

    .VisitProtocolFunctionStatement : (context ProtocolFunctionStatementContext -> v any) {
        id = Visit(context.id()) Result!
        r = Result{}
        ? context.annotationSupport() >< nil {
            r.text += Visit(context.annotationSupport())
        }
        r.permission = "public"
        # 异步
        pout = Visit(context.parameterClauseOut()) str!
        ? context.t.Type == Right_Flow {
            ? pout >< "void" {
                ? context.y >< nil {
                    pout = "\{IEnum}<\{pout}>"
                }
                pout = "\{Task}<\{pout}>"
            } _ {
                pout = Task
            }
            r.text += pout + " " + id.text
        } _ {
            ? context.y >< nil {
                ? pout >< "void" {
                    pout = "\{IEnum}<\{pout}>"
                }
            }
            r.text += pout + " " + id.text
        }
        # 泛型
        templateContract = ""
        ? context.templateDefine() >< nil {
            template = Visit(context.templateDefine()) TemplateItem!
            r.text += template.Template
            templateContract = template.Contract
        }
        r.text += Visit(context.parameterClauseIn()) + templateContract + Terminate + Wrap
        <- r
    }

    .VisitPackageImplementStatement : (context PackageImplementStatementContext -> v any) {
        obj = ""
        ? context.id(0) >< nil {
            Self = Visit(context.id(0)) Result!
            me.selfID = Self.text
        }
        ? context.id(1) >< nil {
            Super = Visit(context.id(1)) Result!
            me.superID = Super.text
        }
        extends = Visit(context.typeType()) str!
        @ item = context.implementSupportStatement().. {
            obj += Visit(item)
        }
        me.selfID = ""
        me.superID = ""
        <- Result{ text = obj; data = extends }
    }
}
