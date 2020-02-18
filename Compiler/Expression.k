Compiler . {
    Antlr4.Runtime
    Antlr4.Runtime.Misc
    System
    System.Collections.Generic

    _ = Compiler.KParser
    _ = Compiler.Compiler_static
}


keywords = {
    "abstract"
    "as"
    "base"
    "bool"
    "break"
    "byte"
    "case"
    "catch"
    "char"
    "checked"
    "class"
    "const"
    "continue"
    "decimal"
    "default"
    "delegate"
    "do"
    "double"
    "enum"
    "event"
    "explicit"
    "extern"
    "false"
    "finally"
    "fixed"
    "float"
    "for"
    "foreach"
    "goto"
    "implicit"
    "in"
    "int"
    "interface"
    "internal"
    "is"
    "lock"
    "long"
    "namespace"
    "new"
    "null"
    "object"
    "operator"
    "out"
    "override"
    "params"
    "private"
    "protected"
    "public"
    "readonly"
    "ref"
    "return"
    "sbyte"
    "sealed"
    "short"
    "sizeof"
    "stackalloc"
    "static"
    "string"
    "struct"
    "switch"
    "this"
    "throw"
    "true"
    "try"
    "typeof"
    "uint"
    "ulong"
    "unchecked"
    "unsafe"
    "ushort"
    "using"
    "virtual"
    "void"
    "volatile"
    "while"
}

TemplateItem : $ {
    Template str
    Contract str
}

DicEle : $ {
    key str
    value str
    text str
}

KLangVisitor : $ (me) {
    .VisitVariableStatement : (context VariableStatementContext -> v any) {
        obj = ""
        r1 = Visit(context.idExpression()) Result!
        r2 = Visit(context.expression()) Result!
        ? context.typeType() >< nil {
            Type = Visit(context.typeType()) str!
            obj = "\{Type} \{r1.text} = \{r2.text; Terminate; Wrap}"
        } _ {
            obj = ? -> r1.isDefine | r1.text == me.selfID | r1.text == me.superID | r1.text == setID {
                "\{r1.text} = \{r2.text; Terminate; Wrap}"
            } _ {
                "var \{r1.text} = \{r2.text; Terminate; Wrap}"
            }
        }
        <- obj
    }

    .VisitVariableDeclaredStatement : (context VariableDeclaredStatementContext -> v any) {
        obj = ""
        Type = Visit(context.typeType()) str!
        r = Visit(context.idExpression()) Result!
        obj = "\{Type} \{r.text; Terminate; Wrap}"
        <- obj
    }

    .VisitAssignStatement : (context AssignStatementContext -> v any) {
        r1 = Visit(context.tupleExpression(0)) Result!
        r2 = Visit(context.tupleExpression(1)) Result!
        obj = r1.text + Visit(context.assign()) + r2.text + Terminate + Wrap
        <- obj
    }

    .VisitAssign : (context AssignContext -> v any) {
        ? context.op.Type == Mod_Equal {
            <- "%="
        }
        <- context.op.Text
    }

    .VisitExpressionStatement : (context ExpressionStatementContext -> v any) {
        r = Visit(context.expression()) Result!
        <- r.text + Terminate + Wrap
    }

    .VisitExpression : (context ExpressionContext -> v any) {
        count = context.ChildCount
        r = Result{}
        ? count.. 3 {
            e1 = Visit(context.GetChild(0)) Result!
            e2 = Visit(context.GetChild(2))
            op = Visit(context.GetChild(1))

            ? context.GetChild(1).. _ CompareCombineContext {
                r.data = I32
                s1 = e1.text
                s2 = (e2 Result!).text
                r.text = "\{s1} > \{s2} ? 1 : ( \{s1}==\{s2} ? 0 : -1 )"
                <- r
            } _ CompareContext {
                # todo 如果左右不是bool类型值，报错
                r.data = Bool
            } _ LogicContext {
                # todo 如果左右不是bool类型值，报错
                r.data = Bool
            } _ AddContext {
                # todo 如果左右不是number或text类型值，报错
                r.data = ? -> e1.data str! == Str | ((e2 Result!).data str!) == Str {
                    Str
                } e1.data str! == I32 & ((e2 Result!).data str!) == I32 {
                    I32
                } _ {
                    F64
                }
            } _ MulContext {
                # todo 如果左右不是number类型值，报错
                r.data = ? -> e1.data str! == I32 & ((e2 Result!).data str!) == I32 {
                    I32
                } _ {
                    F64
                }
            } _ PowContext {
                # todo 如果左右不是number类型，报错
                r.data = F64
                r.text = "\{op}(\{e1.text}, \{(e2 Result!).text})"
                <- r
            }
            r.text = e1.text + op + (e2 Result!).text
        } 2 {
            r = Visit(context.GetChild(0)) Result!
            ? context.GetChild(1) TypeConversionContext? {
                e2 = Visit(context.GetChild(1)) str!
                r.data = e2
                r.text = "(\{e2})(\{r.text})"
            } context.GetChild(1) TypeCheckContext? {
                e2 = Visit(context.GetChild(1)) str!
                r.data = e2
                r.text = "\{r.text}.@is<\{e2}>()"
            } context.GetChild(1) OrElseContext? {
                e2 = Visit(context.GetChild(1)) Result!
                r.text = "(\{r.text}??\{e2.text})"
            } context.GetChild(1) CallExpressionContext? {
                e2 = Visit(context.GetChild(1)) Result!
                r.text = r.text + e2.text
            } context.GetChild(1) CallFuncContext? {
                e2 = Visit(context.GetChild(1)) Result!
                ? me.is_type(r.rootID) {
                    r.text = "(new \{r.text; e2.text})"
                } _ {
                    r.text = r.text + e2.text
                }
            } context.GetChild(1) CallElementContext? {
                e2 = Visit(context.GetChild(1)) Result!
                r.text = r.text + e2.text
            } _ {
                ? context.op.Type == KParser.Bang {
                    r.text = "ref \{r.text}"
                } context.op.Type == KParser.Question {
                    r.text += "?"
                }
            }
        } 1 {
            r = Visit(context.GetChild(0)) Result!
        }
        <- r
    }

    .VisitOrElse : (context OrElseContext -> v any) {
        <- Visit(context.expression()) Result!
    }

    .VisitTypeConversion : (context TypeConversionContext -> v any) {
        <- Visit(context.typeType()) str!
    }

    .VisitTypeCheck : (context TypeCheckContext -> v any) {
        <- Visit(context.typeType()) str!
    }

    .VisitCall : (context CallContext -> v any) {
        <- context.op.Text
    }

    .VisitWave : (context WaveContext -> v any) {
        <- context.op.Text
    }

    .VisitBitwise : (context BitwiseContext -> v any) {
        <- me.Visit(context.GetChild(0)) str!
    }

    .VisitBitwiseAnd : (context BitwiseAndContext -> v any) {
        <- "&"
    }

    .VisitBitwiseOr : (context BitwiseOrContext -> v any) {
        <- "|"
    }

    .VisitBitwiseXor : (context BitwiseXorContext -> v any) {
        <- "^"
    }

    .VisitBitwiseLeftShift : (context BitwiseLeftShiftContext -> v any) {
        <- "<<"
    }

    .VisitBitwiseRightShift : (context BitwiseRightShiftContext -> v any) {
        <- ">>"
    }

    .VisitCompare : (context CompareContext -> v any) {
        ? context.op.Type == Not_Equal {
            <- "!="
        }
        <- context.op.Text
    }

    .VisitLogic : (context LogicContext -> v any) {
        ? context.op.Type == And {
            <- "&&"
        } context.op.Type == Or {
            <- "||"
        }
        <- context.op.Text
    }

    .VisitAdd : (context AddContext -> v any) {
        <- context.op.Text
    }

    .VisitMul : (context MulContext -> v any) {
        ? context.op.Type == Mod {
            <- "%"
        }
        <- context.op.Text
    }

    .VisitPow : (context PowContext -> v any) {
        <- "pow"
    }

    .VisitPrimaryExpression : (context PrimaryExpressionContext -> v any) {
        ? context.ChildCount == 1 {
            c = context.GetChild(0)
            ? c DataStatementContext? {
                <- Visit(context.dataStatement())
            } c IdContext? {
                <- Visit(context.id())
            } context.t.Type == Discard {
                <- Result{
                    text = "_"
                    data = "var"
                }
            }
        } context.ChildCount == 4 {
            id = Visit(context.id()) Result!
            template = Visit(context.templateCall()) str!
            <- Result{
                text = id.text + template
                data = id.text + template
                rootID = id.text
            }
        }
        r = Visit(context.expression()) Result!
        <- Result{
            text = "(\{r.text})"
            data = r.data
        }
    }

    .VisitExpressionList : (context ExpressionListContext -> v any) {
        r = Result{}
        obj = ""
        @ i = 0 ~ context.expression().Length-1.. {
            temp = Visit(context.expression(i)) Result!
            obj += ? -> i == 0 {
                temp.text
            } _ {
                ", " + temp.text
            }
        }
        r.text = obj
        r.data = "var"
        <- r
    }

    .VisitTemplateDefine : (context TemplateDefineContext -> v any) {
        item = TemplateItem{}
        item.Template += "<"
        @ i = 0 ~ context.templateDefineItem().Length-1.. {
            ? i > 0 {
                item.Template += ","
                ? item.Contract.len() > 0 {
                    item.Contract += ","
                }
            }
            r = Visit(context.templateDefineItem(i)) TemplateItem!
            item.Template += r.Template
            item.Contract += r.Contract
        }
        item.Template += ">"
        <- item
    }

    .VisitTemplateDefineItem : (context TemplateDefineItemContext -> v any) {
        item = TemplateItem{}
        ? context.id().len() == 1 {
            id1 = context.id(0).GetText()
            item.Template = id1
        } _ {
            id1 = context.id(0).GetText()
            item.Template = id1
            id2 = context.id(1).GetText()
            item.Contract = " where \{id1}:\{id2}"
        }
        <- item
    }

    .VisitTemplateCall : (context TemplateCallContext -> v any) {
        obj = ""
        obj += "<"
        @ i = 0 ~ context.typeType().Length-1.. {
            ? i > 0 {
                obj += ","
            }
            r = Visit(context.typeType(i))
            obj += r
        }
        obj += ">"
        <- obj
    }

    .VisitDataStatement : (context DataStatementContext -> v any) {
        r = Result{}
        ? context.nilExpr() >< nil {
            r.data = Any
            r.text = "null"
        } context.floatExpr() >< nil {
            r.data = F64
            r.text = Visit(context.floatExpr()) str!
        } context.integerExpr() >< nil {
            r.data = I32
            r.text = Visit(context.integerExpr()) str!
        } context.rawStringExpr() >< nil {
            r.data = Str
            r.text = Visit(context.rawStringExpr()) str!
        } context.stringExpr() >< nil {
            r.data = Str
            r.text = Visit(context.stringExpr()) str!
        } context.t.Type == KParser.CharLiteral {
            r.data = Chr
            r.text = context.CharLiteral().GetText()
        } context.t.Type == KParser.TrueLiteral {
            r.data = Bool
            r.text = T
        } context.t.Type == KParser.FalseLiteral {
            r.data = Bool
            r.text = F
        } 
        <- r
    }

    .VisitStringExpr : (context StringExprContext -> v any) {
        text = ""
        ? context.stringTemplate().Length == 0 {
            @ v = context.stringContent().. {
                text += Visit(v)
            }
            <- "\"\{text}\""
        } _ {
            text = "(new System.Text.StringBuilder()"
            # 去除前后一个元素
            @ i = 1 ~ context.ChildCount-2.. {
                v = context.GetChild(i)
                r = Visit(context.GetChild(i)) str!
                ? v StringContentContext? {
                    text += ".Append(\"\{r}\")"
                } _ {
                    text += r
                }
            }
            text += ").to_str()"
            <- text
        }
    }

    .VisitStringContent : (context StringContentContext -> v any) {
        <- context.TextLiteral().GetText() 
    }

    .VisitStringTemplate : (context StringTemplateContext -> v any) {
        text = ""
        @ v = context.expression().. {
            r = Visit(v) Result!
            text += ".Append(\{r.text})"
        }
        <- text
    }

    .VisitRawStringExpr : (context RawStringExprContext -> v any) {
        text = ""
        ? context.rawStringTemplate().Length == 0 {
            # 去除前后一个元素
            @ i = 1 ~ context.ChildCount-2.. {
                v = context.GetChild(i)
                r = Visit(context.GetChild(i)) str!
                ? v RawStringContentContext? {
                    text += r
                } _ {
                    text += "\"\""
                }
            }
            <- "@\"\{text}\""
        } _ {
            text = "(new System.Text.StringBuilder()"
            # 去除前后一个元素
            @ i = 1 ~ context.ChildCount-2.. {
                v = context.GetChild(i)
                r = Visit(context.GetChild(i)) str!
                ? v RawStringContentContext? {
                    text += ".Append(@\"\{r}\")"
                } v RawStringTemplateContext? {
                    text += r
                } _ {
                    text += ".Append('\"')"
                }
            }
            text += ").to_str()"
            <- text
        }
    }

    .VisitRawStringContent : (context RawStringContentContext -> v any) {
        <- context.RawTextLiteral().GetText()
    }

    .VisitRawStringTemplate : (context RawStringTemplateContext -> v any) {
        text = ""
        @ v = context.expression().. {
            r = Visit(v) Result!
            text += ".Append(\{r.text})"
        }
        <- text
    }

    .VisitFloatExpr : (context FloatExprContext -> v any) {
        number = context.FloatLiteral().GetText()
        <- number
    }

    .VisitIntegerExpr : (context IntegerExprContext -> v any) {
        number = context.GetChild(0).GetText()
        <- number
    }

    .VisitPlusMinus : (context PlusMinusContext -> v any) {
        r = Result{}
        expr = Visit(context.expression()) Result!
        op = Visit(context.add())
        r.data = expr.data
        r.text = op + expr.text
        <- r
    }

    .VisitNegate : (context NegateContext -> v any) {
        r = Result{}
        expr = Visit(context.expression()) Result!
        r.data = expr.data
        r.text = "!" + expr.text
        <- r
    }

    .VisitBitwiseNotExpression : (context BitwiseNotExpressionContext -> v any) {
        r = Result{}
        expr = Visit(context.expression()) Result!
        r.data = expr.data
        r.text = "~" + expr.text
        <- r
    }

    .VisitLinq : (context LinqContext -> v any) {
        r = Result{data = "var"}
        r.text += Visit(context.linqHeadItem()) str!
        @ item = context.linqItem().. {
            r.text += "\{Visit(item)} "
        }
        r.text += "\{(Visit(context.id()) Result!).text} \{(Visit(context.expression()) Result!).text}"
        <- r
    }

    .VisitLinqItem : (context LinqItemContext -> v any) {
        ? context.linqHeadItem() >< nil {
            <- Visit(context.linqHeadItem()) str!
        }
        obj = (Visit(context.id()) Result!).text
        ? context.expression() >< nil {
            obj += " \{(Visit(context.expression()) Result!).text}"
        }
        <- obj
    }

    .VisitLinqHeadItem : (context LinqHeadItemContext -> v any) {
        obj = ""
        id = Visit(context.id()) Result!
        obj += "from \{id.text} in \{(Visit(context.expression()) Result!).text} "
        <- obj
    }
}
