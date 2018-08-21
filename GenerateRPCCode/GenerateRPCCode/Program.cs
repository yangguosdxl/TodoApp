using CoolRpcInterface;
using MessagePack;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using CSharpSyntax;
using System.IO;
using RpcTestInterface;

namespace GenerateRPCCode
{
    class Program
    {
        static EnumDeclarationSyntax s_EDSProtoID;
        static CompilationUnitSyntax s_CompilationUnitSyntax;

        static void Main(string[] args)
        {
            List<SyntaxNode> nodes = new List<SyntaxNode>();

            s_CompilationUnitSyntax = new CompilationUnitSyntax
            {

            };

            s_EDSProtoID = Syntax.EnumDeclaration(identifier: "ProtoID", modifiers: Modifiers.Public);

            foreach (Type t in typeof(IHelloService).Assembly.GetTypes())
            {
                if (t.IsInterface && typeof(ICoolRpc).IsAssignableFrom(t))
                {
                    var nodes2 = ParseInterface(t);
                    nodes.AddRange(nodes2);
                }
            }

            string s = GenerateCode(nodes, new CSharpSyntax.Printer.Configuration.SyntaxPrinterConfiguration());
            Console.Write(s);
        }

        protected static string GenerateCode(IEnumerable<SyntaxNode> nodes, CSharpSyntax.Printer.Configuration.SyntaxPrinterConfiguration configuration)
        {
            using (var writer = new StringWriter())
            {
                foreach (var node in nodes)
                {
                    using (var printer = new CSharpSyntax.Printer.SyntaxPrinter(new CSharpSyntax.Printer.SyntaxWriter(writer, configuration)))
                    {
                        printer.Visit(node);
                    }
                }

                return writer.GetStringBuilder().ToString();
            }
        }


        static List<SyntaxNode> ParseInterface(Type t)
        {
            if (t.Name.StartsWith("I") == false)
            {
                throw new Exception("rpc interface must start with I: " + t.Name);
            }

            List<SyntaxNode> nodes = new List<SyntaxNode>();

            ClassDeclarationSyntax classRpcImpl = new ClassDeclarationSyntax
            {
                Identifier = t.Name.Substring(1),
                BaseList = new BaseListSyntax
                {
                    Types =
                        {
                            Syntax.ParseName(t.Name)
                        }
                }
            };
            nodes.Add(classRpcImpl);

            classRpcImpl.Members.Add(Syntax.PropertyDeclaration(
                    modifiers: Modifiers.Public,
                    identifier: "CallAsync",
                    type: Syntax.ParseName("ICallAsync"),
                    accessorList: new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                                Body = new BlockSyntax()
                            },
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                    ));

            classRpcImpl.Members.Add(Syntax.PropertyDeclaration(
                    modifiers: Modifiers.Public,
                    identifier: "Serializer",
                    type: Syntax.ParseName("ICallAsync"),
                    accessorList: new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                                Body = new BlockSyntax()
                            },
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                    ));

            classRpcImpl.Members.Add(Syntax.ConstructorDeclaration(
                modifiers: Modifiers.Public,
                identifier: classRpcImpl.Identifier,
                parameterList: new ParameterListSyntax()
                {
                    Parameters =
                        {
                            new ParameterSyntax
                            {
                                Identifier = "callAsync",
                                Type = Syntax.ParseName("ICallAsync")
                            },
                            new ParameterSyntax
                            {
                                Identifier = "serializer",
                                Type = Syntax.ParseName("ISerializer")
                            }
                        }
                },
                body: new BlockSyntax()
                {
                    Statements =
                    {
                        Syntax.ExpressionStatement(Syntax.BinaryExpression(
                            @operator: BinaryOperator.Equals,
                            left: Syntax.ParseName("CallAsync"),
                            right: Syntax.ParseName("callAsync")
                            )),
                        Syntax.ExpressionStatement(Syntax.BinaryExpression(
                            @operator: BinaryOperator.Equals,
                            left: Syntax.ParseName("CallAsync"),
                            right: Syntax.ParseName("callAsync")
                            ))
                    }
                }
                ));

            foreach (MethodInfo mi in t.GetMethods())
            {
                var nodes2 = ParseInterfaceMethod(classRpcImpl, mi);
                nodes.AddRange(nodes2);
            }
            return nodes;
        }

        static AttributeListSyntax GetMsgAttribute()
        {
            return new AttributeListSyntax
            {
                Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("MessagePack.MessagePackObject")
                        }
                    }
            };
        }

        static AttributeListSyntax GetMsgFieldAttribute(int key)
        {
            return new AttributeListSyntax
            {
                Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("MessagePack.Key"),
                            ArgumentList = new AttributeArgumentListSyntax
                            {
                                Arguments =
                                {
                                    new AttributeArgumentSyntax
                                    {
                                        Expression = Syntax.LiteralExpression(key)
                                    }
                                }
                            }
                        }
                    }
            };
        }

        static string GetRetValues(Type typeRet)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ValueTuple<");
            foreach (Type t in typeRet.GenericTypeArguments[0].GenericTypeArguments)
            {
                sb.Append(t.ToString()).Append(",");
            }
            sb.Remove(sb.Length - 1, 1).Append(">");

            return sb.ToString();
        }


        static List<SyntaxNode> ParseInterfaceMethod(ClassDeclarationSyntax rpcClassImplNode, MethodInfo mi)
        {
            List<SyntaxNode> nodes = new List<SyntaxNode>();

            string szMiName = mi.Name;

            ParameterListSyntax rpcInParams = new ParameterListSyntax();

            // 入参消息

            StructDeclarationSyntax MsgInStruct = new StructDeclarationSyntax
            {
                Identifier = szMiName + "MsgIn"
            };
            MsgInStruct.AttributeLists.Add(GetMsgAttribute());
            nodes.Add(MsgInStruct);

            // 入参
            ParameterInfo[] aPiIn = mi.GetParameters();
            int i = 0;
            foreach(var pi in aPiIn)
            {
                ++i;
                var field = Syntax.FieldDeclaration(
                    modifiers: Modifiers.Public,
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName(pi.ParameterType.ToString()),
                        new[] { Syntax.VariableDeclarator(pi.Name) }
                    )
                );
                field.AttributeLists.Add(GetMsgFieldAttribute(i));
                MsgInStruct.Members.Add(field);

                rpcInParams.Parameters.Add(Syntax.Parameter(
                    identifier: pi.Name,
                    type: Syntax.ParseName(pi.ParameterType.Name)
                    ));
            }

            // 出参
            bool bIsReturnValue = true;
            Type typeRet = mi.ReturnType;

            StructDeclarationSyntax MsgOutStruct = new StructDeclarationSyntax
            {
                Identifier = szMiName + "MsgOut"
            };

            if (typeRet == typeof(Task))
            {
                bIsReturnValue = false;
            }
            else if (typeRet.GetGenericTypeDefinition() == (typeof(Task<>)) && typeRet.GenericTypeArguments[0].GetGenericTypeDefinition().ToString().StartsWith("System.ValueTuple"))
            {
                MsgOutStruct.AttributeLists.Add(GetMsgAttribute());
                nodes.Add(MsgOutStruct);


                string retValues = GetRetValues(typeRet);

                var field = Syntax.FieldDeclaration(
                    modifiers: Modifiers.Public,
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName(retValues),
                        new[] { Syntax.VariableDeclarator("Value") }
                    )
                );
                field.AttributeLists.Add(GetMsgFieldAttribute(1));
                MsgOutStruct.Members.Add(field);
            }
            else
            {
                Console.WriteLine("Must return Task or Task<>");
                return nodes;
            }


            // 函数名
            MethodDeclarationSyntax rpcCallMDS = new MethodDeclarationSyntax
            {
                Modifiers = Modifiers.Public | Modifiers.Async,
                Identifier = szMiName,
                ParameterList = rpcInParams,
                Body = new BlockSyntax()
            };
            rpcClassImplNode.Members.Add(rpcCallMDS);

            string sendMsgSyntax = "";
            if (bIsReturnValue)
            {
                string retValues = GetRetValues(typeRet);
                rpcCallMDS.ReturnType = Syntax.ParseName(string.Format("Task<{0}>", retValues));

                sendMsgSyntax = "CallAsync.SendWithResponse";
            }
            else
            {
                rpcCallMDS.ReturnType = Syntax.ParseName(typeRet.Name);

                sendMsgSyntax = "CallAsync.SendWithoutResponse";
            }

            rpcCallMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName(MsgInStruct.Identifier), 
                    new[] {Syntax.VariableDeclarator(
                        "msg",
                        initializer: Syntax.EqualsValueClause(
                            Syntax.ObjectCreationExpression(
                            Syntax.ParseName(MsgInStruct.Identifier),
                            Syntax.ArgumentList()
                            )
                        )) })));
            // 给msg赋值
            foreach (var pi in aPiIn)
            {
                rpcCallMDS.Body.Statements.Add(Syntax.ExpressionStatement(Syntax.BinaryExpression(
                    @operator: BinaryOperator.Equals,
                    left: Syntax.ParseName("msg." + pi.Name),
                    right: Syntax.ParseName(pi.Name)
                    )));
            }

            // var (bytes, iStart, len ) = m_Serializer.Serialize(msg);
            rpcCallMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "msgSerializeInfo",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.InvocationExpression(
                                            expression: new MemberAccessExpressionSyntax
                                            {
                                                Expression = Syntax.ParseName("Serializer"),
                                                Name = (SimpleNameSyntax)Syntax.ParseName("Serialize")
                                            },
                                            argumentList: Syntax.ArgumentList(Syntax.Argument(Syntax.ParseName("msg")))
                                        )
                                    )) }
                    )
                 )
             );

            // var (byteRet, indexRet, lenRet) = await m_CallAsync.SendWithResponse(bytes, iStart, len);
            var sendExpression = Syntax.AwaitExpression(Syntax.InvocationExpression(
                expression: Syntax.ParseName(sendMsgSyntax),
                argumentList: Syntax.ArgumentList(
                    Syntax.Argument(Syntax.ParseName("msgSerializeInfo.item1")),
                    Syntax.Argument(Syntax.ParseName("msgSerializeInfo.item2")),
                    Syntax.Argument(Syntax.ParseName("msgSerializeInfo.item3"))
                    )
                ));
            rpcCallMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "ret",
                                    initializer: Syntax.EqualsValueClause(
                                        sendExpression
                                    )) }
                    )
                 )
             );

            if (bIsReturnValue)
            {
                // var retMsg = m_Serializer.Deserialize<MsgHelloIntRet>(byteRet, indexRet, lenRet);
                rpcCallMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "retMsg",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.InvocationExpression(
                                            Syntax.ParseName(string.Format("Serializer.Deserialize<{0}>", MsgOutStruct.Identifier)),
                                            argumentList: Syntax.ArgumentList(
                                                Syntax.Argument(Syntax.ParseName("ret.item1")),
                                                Syntax.Argument(Syntax.ParseName("ret.item2")),
                                                Syntax.Argument(Syntax.ParseName("ret.item3"))
                                            )
                                        )
                                    )
                          )}
                    )
                ));

                // return await Task.FromResult((ret.a, ret.a));
                rpcCallMDS.Body.Statements.Add(Syntax.ReturnStatement(
                    Syntax.InvocationExpression(
                        expression: Syntax.ParseName("Task.FromResult"),
                        argumentList: Syntax.ArgumentList(
                            Syntax.Argument(Syntax.ParseName("retMsg.Value"))
                            )
                        )
                    ));
                //rpcCallMDS.Body.Statements.Add(Syntax.ReturnStatement(
                //    Syntax.AwaitExpression(Syntax.InvocationExpression(
                //        expression: Syntax.ParseName("Task.FromResult"),
                //        argumentList: Syntax.ArgumentList(
                //            Syntax.Argument(Syntax.ParseName("retMsg.Value")),
                //            Syntax.Argument(Syntax.ParseName("msgSerializeInfo.item2")),
                //            Syntax.Argument(Syntax.ParseName("msgSerializeInfo.item3"))
                //            )
                //        ))
                //    ));
            }
            else
            {
                rpcCallMDS.Body.Statements.Add(Syntax.ReturnStatement(
                    Syntax.ParseName("ret")
                    ));
            }




            return nodes;
        }
    }
}
