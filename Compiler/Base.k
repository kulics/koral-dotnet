Compiler . {
    Antlr4.Runtime
    Antlr4.Runtime.Misc
    System
    System.Collections.Generic

    _ : Compiler.KParser
    _ : Compiler.Compiler_static
}

Terminate : ";"
Wrap : "\r\n"

Any : "object"
Int : "int"
Num : "double"
I8 : "sbyte"
I16 : "short"
I32 : "int"
I64 : "long"

U8 : "byte"
U16 : "ushort"
U32 : "uint"
U64 : "ulong"

F32 : "float"
F64 : "double"

Bool : "bool"
T : "true"
F : "false"

Chr : "char"
Str : "string"
Lst : "list"
Set : "hashset"
Dic : "dictionary"
Stk : "stack"
Que : "queue"

BlockLeft : "{"
BlockRight : "}"

Task : "System.Threading.Tasks.Task"
IEnum : "System.Collections.Generic.IEnumerable"

Result : $ {
    data! any
    text! str
    permission! str
    isVirtual! bool
    isDefine! bool
    rootID! str : ""
}

KLangVisitor : [KParserBaseVisitor any] & $me {
    selfID! : ""
    superID! : ""
    setID! : ""
    getID! : ""
    selfPropertyID! : ""
    selfPropertyContent! : []str{}
    selfPropertyVariable! : false

    AllIDSet! : [str]_{}
    CurrentIDSet! : [>][str]_{}
    TypeIdSet! : [str]_{}

    has_id : (id str -> v bool) {
        <- me.AllIDSet.contains(id) | me.CurrentIDSet.peek().contains(id)
    }
    add_id : (id str ->) {
        me.CurrentIDSet.peek().add(id)
    }
    add_current_set : (->) {
        @ item : CurrentIDSet.peek().. {
            AllIDSet.add(item)
        }
        me.CurrentIDSet.push([str]_{})
    }
    delete_current_set : (->) {
        me.AllIDSet.except_with(me.CurrentIDSet.peek())
        me.CurrentIDSet.pop()
    }
    is_type : (id str->v bool) {
        <- me.TypeIdSet.contains(id)
    }
    add_type : (id str ->) {
        me.TypeIdSet.add(id)
    }
} & (->$me) {
    me.CurrentIDSet.push([str]_{})
}

KLangVisitor : $ me {
    .VisitProgram : (context ProgramContext -> v any) {
        StatementList : context.statement()
        Result! : ""
        @ item : StatementList.. {
            Result += VisitStatement(item)
        }
        <- Result
    }

    .VisitId : (context IdContext -> v any) {
        r : Result{data = "var"}
        first : Visit(context.GetChild(0)) Result!
        r.permission = first.permission
        r.text = first.text
        r.isVirtual = first.isVirtual
        ? context.ChildCount >= 2 {
            @ i : 1 ~ context.ChildCount-1.. {
                other : Visit(context.GetChild(i)) Result!
                r.text += "_${other.text}"
            }
        }

        ? keywords.Exists{t -> t == r.text} {
            r.text = "@${r.text}"
        }
        ? r.text == selfID {
            r.text = "this"
        } r.text == superID {
            r.text = "base"
        } r.text == setID {
            r.text = "value"
        } r.text == getID {
            r.text = "_" + selfPropertyID
        }
        r.rootID = r.text
        <- r
    }

    .VisitIdItem : (context IdItemContext -> v any) {
        r : Result{data = "var"}
        ? context.typeBasic() >< nil {
            r.permission = "public"
            r.text += context.typeBasic().GetText()
            r.isVirtual = true
        } context.typeAny() >< nil {
            r.permission = "public"
            r.text += context.typeAny().GetText()
            r.isVirtual = true
        } context.op.Type == IDPublic {
            r.permission = "public"
            r.text += context.op.Text
            r.isVirtual = true
        } context.op.Type == IDPrivate {
            r.permission = "protected"
            r.text += context.op.Text
            r.isVirtual = true
        }
        <- r
    }

    # .VisitIdExpression : (context IdExpressionContext -> v any) {
    #     r = Result{data = "var"}
    #     ? context.idExprItem().Length > 1 {
    #         r.text = "("
    #         @ [i]v = context.idExprItem().. {
    #             subID = (Visit(v) Result!).text
    #             r.text += ? -> i >< 0 {
    #                 ", " + subID
    #             } _ {
    #                 subID
    #             }
    #             ? me.has_id(subID) {
    #                 r.isDefine = true
    #             } _ {
    #                 me.add_id(subID)
    #             }
    #         }
    #         r.text += ")"
    #     } _ {
    #         r = Visit(context.idExprItem(0)) Result!
    #         ? me.has_id(r.text) {
    #             r.isDefine = true
    #         } _ {
    #             me.add_id(r.text)
    #         }
    #     }
    #     <- r
    # }

    .VisitVarId : (context VarIdContext -> v any) {
        ? context.Discard() >< nil {
            <- "_"
        } _ {
            id : (Visit(context.id()) Result!).text
            ? me.has_id(id) {
                # r.isDefine = true
            } _ {
                me.add_id(id)
            }
            ? context.typeType() >< nil {
                <- Visit(context.typeType()) + " " + id
            } _ {
                <- "var " + id
            }
        }
    }

    .VisitConstId : (context ConstIdContext -> v any) {
        ? context.Discard() >< nil {
            <- "_"
        } _ {
            id : (Visit(context.id()) Result!).text
            ? me.has_id(id) {
                # r.isDefine = true
            } _ {
                me.add_id(id)
            }
            ? context.typeType() >< nil {
                <- Visit(context.typeType()) + " " + id
            } _ {
                <- "var " + id
            }
        }
    }

    .VisitBoolExpr : (context BoolExprContext -> v any) {
        r : Result{}
        ? context.t.Type == TrueLiteral {
            r.data = Bool
            r.text = T
        } context.t.Type == FalseLiteral {
            r.data = Bool
            r.text = F
        }
        <- r
    }

    .VisitAnnotationSupport : (context AnnotationSupportContext -> v any) {
        <- Visit(context.annotation()) str!
    }

    .VisitAnnotation : (context AnnotationContext -> v any) {
        obj! : ""
        id! : ""
        ? context.id() >< nil {
            id = "${(Visit(context.id()) Result!).text}:"
        }

        r : Visit(context.annotationList()) str!
        ? r >< "" {
            obj += "[${id; r}]"
        }
        <- obj
    }

    .VisitAnnotationList : (context AnnotationListContext -> v any) {
        obj! : ""
        @ [i]v : context.annotationItem().. {
            txt : me.Visit(v) str!
            ? txt >< "" {
                obj += ? -> i > 0 {
                    "," + txt
                } _ {
                    txt
                }
            }
        }
        <- obj
    }

    .VisitAnnotationItem : (context AnnotationItemContext -> v any) {
        obj! : ""
        obj += (me.Visit(context.id()) Result!).text
        ? obj.. "get" {
            ? context.lambda() == nil {
                me.selfPropertyVariable = true
                me.selfPropertyContent += "get{return _${ me.selfPropertyID }; }"
            } _ {
                me.selfPropertyContent += "get{${ me.VisitPropertyLambda(context.lambda(), true) }}"
            }
            <- ""
        } "set" {
            ? context.lambda() == nil {
                me.selfPropertyVariable = true
                me.selfPropertyContent += "set{_${ me.selfPropertyID }=value;}"
            } _ {
                me.selfPropertyContent += "set{${ me.VisitPropertyLambda(context.lambda(), false) }}"
            }
            <- ""
        } "_get" {
            ? context.lambda() == nil {
                me.selfPropertyVariable = true
                me.selfPropertyContent += "private get{return _${ me.selfPropertyID }; }"
            } _ {
                me.selfPropertyContent += "private get{${ me.VisitPropertyLambda(context.lambda(), true) }}"
            }
            <- ""
        } "_set" {
            ? context.lambda() == nil {
                me.selfPropertyVariable = true
                me.selfPropertyContent += "private set{_${ me.selfPropertyID }=value;}"
            } _ {
                me.selfPropertyContent += "private set{${ me.VisitPropertyLambda(context.lambda(), false) }}"
            }
            <- ""
        } "add" {
            todo("not yet")
            <- ""
        } "remove" {
            todo("not yet")
            <- ""
        }
        obj += ? -> context.tuple() >< nil {
            (me.Visit(context.tuple()) Result!).text
        } context.lambda() >< nil {
            "(${ (me.Visit(context.lambda()) Result!).text })"
        } _ {
            ""
        }
        <- obj
    }

    VisitPropertyLambda : (context LambdaContext, is_get bool -> v str) {
        me.add_current_set()
        obj! : ""
        ? context.lambdaIn() >< nil {
            me.VisitPropertyLambdaIn(context.lambdaIn(), is_get)
        }

        ? context.tupleExpression() >< nil {
            obj += (Visit(context.tupleExpression()) Result!).text
            ? is_get {
                obj = "return " + obj
            }
            obj += Terminate
        } _ {
            obj += ProcessFunctionSupport(context.functionSupportStatement())
        }
        me.getID = ""
        me.setID = ""
        me.delete_current_set()
        <- obj
    }

    VisitPropertyLambdaIn : (context LambdaInContext, is_get bool ->) {
        ? context.id().Length.. 1 {
            id0 : me.Visit(context.id(0)) Result!
            me.add_id(id0.text)
            ? is_get {
                me.selfPropertyVariable = true
                me.add_id("_"+me.selfPropertyID)
                me.getID = id0.text
            } _ {
                me.setID = id0.text
            }
        } 2 {
            me.selfPropertyVariable = true
            me.add_id("_"+me.selfPropertyID)
            id0 : me.Visit(context.id(0)) Result!
            id1 : me.Visit(context.id(1)) Result!
            me.add_id(id0.text)
            me.add_id(id1.text)
            me.getID = id0.text
            me.setID = id1.text
        }
    }
}
