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
using Cool.Coroutine;

namespace GenerateRPCCode
{
    class Program
    {
        static EnumDeclarationSyntax s_EDSProtoID;
        static CompilationUnitSyntax s_CompilationUnitSyntax;
        static NamespaceDeclarationSyntax s_NameSpace;
        static ClassDeclarationSyntax s_CDSRegistAllRpcService;
        static BlockSyntax s_AddAllRpcServicesBody;
        static BlockSyntax s_HandlerMapConstructorBody;

        static void Main(string[] args)
        {
            List<SyntaxNode> nodes = new List<SyntaxNode>();
            List<BaseTypeDeclarationSyntax> aGenTypes = new List<BaseTypeDeclarationSyntax>();

            s_CompilationUnitSyntax = Syntax.CompilationUnit(

                usings: new[]
                    {
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("CoolRpcInterface")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System.Threading.Tasks")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("Microsoft.Extensions.DependencyInjection")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("Cool.Coroutine")),
                    }
            );
            s_NameSpace = Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("CSRPC"));
            s_CompilationUnitSyntax.Members.Add(s_NameSpace);

            s_EDSProtoID = Syntax.EnumDeclaration(identifier: "ProtoID", modifiers: Modifiers.Public);
            aGenTypes.Add(s_EDSProtoID);
            

            s_CDSRegistAllRpcService = new ClassDeclarationSyntax
            {
                Modifiers = Modifiers.Public | Modifiers.Static,
                Identifier = "RpcServicesConfiguration",
            };
            MethodDeclarationSyntax mdsAddAllRpcServices = new MethodDeclarationSyntax
            {
                Modifiers = Modifiers.Public | Modifiers.Static,
                ReturnType = Syntax.ParseName("void"),
                Identifier = "AddAllRpcServices",
                ParameterList = Syntax.ParameterList(Syntax.Parameter(
                    type: Syntax.ParseName("IServiceCollection"),
                    identifier: "ServiceCollection"
                    )),
                Body = new BlockSyntax()
            };
            //mdsAddAllRpcServices.AttributeLists.Add(Syntax.AttributeList(
            //    attributes: new[] { Syntax.Attribute((NameSyntax)Syntax.ParseName("Extension")) }));
            s_AddAllRpcServicesBody = mdsAddAllRpcServices.Body;
            s_CDSRegistAllRpcService.Members.Add(mdsAddAllRpcServices);
            aGenTypes.Add(s_CDSRegistAllRpcService);

            foreach (Type t in typeof(RpcTestInterface.RpcTestInterface).Assembly.GetTypes())
            {
                if (t.IsInterface && typeof(ICoolRpc).IsAssignableFrom(t))
                {
                    Console.WriteLine($"PROCESS {t.ToString()}");
                    var aGenTypes2 = ParseInterface(t);

                    aGenTypes.AddRange(aGenTypes2);
                }
            }

            var protoIDCount = Syntax.EnumMemberDeclaration("COUNT");
            s_EDSProtoID.Members.Add(protoIDCount);

            nodes.Add(s_CompilationUnitSyntax);

            foreach(BaseTypeDeclarationSyntax genType in aGenTypes)
            {
                s_NameSpace.Members.Clear();
                s_NameSpace.Members.Add(genType);
                string s = GenerateCode(nodes, new CSharpSyntax.Printer.Configuration.SyntaxPrinterConfiguration());
                Console.Write(s);
                Console.Write(Directory.GetCurrentDirectory());
                File.WriteAllText($"../../../RpcTestImpl/{genType.Identifier}.cs", s);
            }

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


        static List<TypeDeclarationSyntax> ParseInterface(Type t)
        {
            if (t.Name.StartsWith("I") == false)
            {
                throw new Exception("rpc interface must start with I: " + t.Name);
            }

            List<TypeDeclarationSyntax> nodes = new List<TypeDeclarationSyntax>();

            #region RPC代理类
            ClassDeclarationSyntax classRpcImpl = new ClassDeclarationSyntax
            {
                Modifiers = Modifiers.Public,
                Identifier = t.Name.Substring(1),
                BaseList = new BaseListSyntax
                {
                    Types =
                        {
                            Syntax.ParseName(t.FullName)
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
                                //Body = new BlockSyntax()
                            },
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                //Body = new BlockSyntax()
                            }
                        }
                    }
                    ));

            classRpcImpl.Members.Add(Syntax.PropertyDeclaration(
                    modifiers: Modifiers.Public,
                    identifier: "Serializer",
                    type: Syntax.ParseName("ISerializer"),
                    accessorList: new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                                //Body = new BlockSyntax()
                            },
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                //Body = new BlockSyntax()
                            }
                        }
                    }
                    ));

            classRpcImpl.Members.Add(Syntax.PropertyDeclaration(
                    modifiers: Modifiers.Public,
                    identifier: "ChunkType",
                    type: Syntax.ParseName("int"),
                    accessorList: new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                            },
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                            }
                        }
                    }
                    ));
            #endregion


            #region 协议处理类
            ClassDeclarationSyntax classHandlerMapImpl = new ClassDeclarationSyntax
            {
                Modifiers = Modifiers.Public,
                Identifier = t.Name + "_HandlerMap",
                BaseList = new BaseListSyntax
                {
                    Types =
                        {
                            Syntax.ParseName("IRPCHandlerMap")
                        }
                }
            };
            nodes.Add(classHandlerMapImpl);

            classHandlerMapImpl.Members.Add(Syntax.FieldDeclaration(
                    modifiers: Modifiers.Private,
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName(t.ToString()),
                        new[] { Syntax.VariableDeclarator("m_service") }
                    )
                ));

            s_HandlerMapConstructorBody = new BlockSyntax();
            classHandlerMapImpl.Members.Add(Syntax.ConstructorDeclaration(
                    modifiers: Modifiers.Public,
                    identifier: classHandlerMapImpl.Identifier,
                    body: s_HandlerMapConstructorBody,
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter(
                            type: Syntax.ParseName(t.ToString()), identifier: "service"
                            )
                        )/*,
                    initializer: Syntax.ConstructorInitializer(ThisOrBase.Base, Syntax.ArgumentList(
                        Syntax.Argument(Syntax.CastExpression("int", Syntax.ParseName(s_EDSProtoID.Identifier + ".COUNT")))
                        ))*/

                    ));
            s_HandlerMapConstructorBody.Statements.Add(Syntax.ExpressionStatement(Syntax.BinaryExpression(
                BinaryOperator.Equals,
                Syntax.ParseName("m_service"),
                Syntax.ParseName("service")
            )));

            #endregion

#if false
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
                            left: Syntax.ParseName("Serializer"),
                            right: Syntax.ParseName("serializer")
                            ))
                    }
                }
                ));
#endif

            s_AddAllRpcServicesBody.Statements.Add(Syntax.ExpressionStatement(
                Syntax.InvocationExpression(
                Syntax.ParseName(string.Format("ServiceCollection.AddSingleton<{0}, {1}>", t.FullName, classRpcImpl.Identifier)),
                Syntax.ArgumentList()
                )));

            foreach (MethodInfo mi in t.GetMethods())
            {
                var aNewTypes = ParseInterfaceMethod(classRpcImpl, classHandlerMapImpl, t, mi);
                nodes.AddRange(aNewTypes);
            }

            return nodes;
        }

        


        static List<TypeDeclarationSyntax> ParseInterfaceMethod(ClassDeclarationSyntax rpcClassImplNode, ClassDeclarationSyntax handlerMapClassImplNode, Type t, MethodInfo mi)
        {
            List<TypeDeclarationSyntax> nodes = new List<TypeDeclarationSyntax>();

            string szMiName = mi.Name;

            #region RPC代理类
            ParameterListSyntax rpcInParams = new ParameterListSyntax();

            // 入参消息
            var msgInProtoID = Syntax.EnumMemberDeclaration("E" + t.Name + "_" + szMiName + "_MsgIn");
            var msgOutProtoID = Syntax.EnumMemberDeclaration("E" + t.Name + "_" + szMiName + "_MsgOut");
            s_EDSProtoID.Members.Add(msgInProtoID);

            ClassDeclarationSyntax MsgInStruct = new ClassDeclarationSyntax
            {
                Modifiers = Modifiers.Public,
                Identifier = t.Name + "_" + szMiName + "_MsgIn"
            };
            MsgInStruct.AttributeLists.Add(GetMsgAttribute());
            nodes.Add(MsgInStruct);

            //{
            //    var field = Syntax.FieldDeclaration(
            //        modifiers: Modifiers.Public,
            //        declaration: Syntax.VariableDeclaration(
            //            Syntax.ParseName(s_EDSProtoID.Identifier),
            //            new[] { Syntax.VariableDeclarator(
            //                identifier: "eProtoID",
            //                initializer: Syntax.EqualsValueClause(Syntax.ParseName("ProtoID."+msgInProtoID.Identifier))
            //                ) }
            //        )
            //    );
            //    field.AttributeLists.Add(GetMsgFieldAttribute(1));
            //    MsgInStruct.Members.Add(field);
            //}


            // 入参
            ParameterInfo[] aPiIn = mi.GetParameters();
            int i = 0;
            foreach (var pi in aPiIn)
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
                    type: Syntax.ParseName(pi.ParameterType.ToString())
                    ));
            }

            // 出参
            bool bHasReturnValue = true;
            Type typeRet = mi.ReturnType;

            ClassDeclarationSyntax MsgOutStruct = new ClassDeclarationSyntax
            {
                Modifiers = Modifiers.Public,
                Identifier = t.Name + "_" + szMiName + "_MsgOut"
            };

            if (typeRet == typeof(void))
            {
                bHasReturnValue = false;
            }
            else if (typeRet.GetGenericTypeDefinition() == (typeof(MyTask<>)))
            {
                s_EDSProtoID.Members.Add(msgOutProtoID);

                MsgOutStruct.AttributeLists.Add(GetMsgAttribute());
                nodes.Add(MsgOutStruct);

                //{
                //    var field = Syntax.FieldDeclaration(
                //        modifiers: Modifiers.Public,
                //        declaration: Syntax.VariableDeclaration(
                //            Syntax.ParseName(s_EDSProtoID.Identifier),
                //            new[] { Syntax.VariableDeclarator(
                //            identifier: "eProtoID",
                //            initializer: Syntax.EqualsValueClause(Syntax.ParseName("ProtoID."+msgOutProtoID.Identifier))
                //            ) }
                //        )
                //    );
                //    field.AttributeLists.Add(GetMsgFieldAttribute(1));
                //    MsgOutStruct.Members.Add(field);
                //}

                string retValues = GetRetValues(typeRet);
                {
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

            }
            else
            {
                Console.WriteLine("Must return Task or Task<>");
                return nodes;
            }


            // 函数名
            // public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
            MethodDeclarationSyntax rpcCallMDS = new MethodDeclarationSyntax
            {
                Modifiers = bHasReturnValue ?  Modifiers.Public | Modifiers.Async : Modifiers.Public,
                Identifier = szMiName,
                ParameterList = rpcInParams,
                Body = new BlockSyntax()
            };
            rpcClassImplNode.Members.Add(rpcCallMDS);

            string sendMsgSyntax = "";
            if (bHasReturnValue)
            {
                string retValues = GetRetValues(typeRet);
                rpcCallMDS.ReturnType = Syntax.ParseName(string.Format("MyTask<{0}>", retValues));

                sendMsgSyntax = $"CallAsync.SendWithResponse<{MsgOutStruct.Identifier}>";
            }
            else
            {
                rpcCallMDS.ReturnType = Syntax.ParseName("void");

                sendMsgSyntax = "CallAsync.SendWithoutResponse";
            }

            // ICHelloService_HelloInt_MsgIn msg = new ICHelloService_HelloInt_MsgIn();
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
            // msg.a = a;
            foreach (var pi in aPiIn)
            {
                rpcCallMDS.Body.Statements.Add(Syntax.ExpressionStatement(Syntax.BinaryExpression(
                    @operator: BinaryOperator.Equals,
                    left: Syntax.ParseName("msg." + pi.Name),
                    right: Syntax.ParseName(pi.Name)
                    )));
            }

            // Func<byte[], int, (byte[], int,int)> f = delegate(byte[] buffer, int start) {}
            BlockSyntax serializeBlock = Syntax.Block();

            rpcCallMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("Func<byte[], int, ValueTuple<byte[], int,int>>"),
                    new[] {Syntax.VariableDeclarator(
                        "f",
                        initializer: Syntax.EqualsValueClause(
                            Syntax.AnonymousMethodExpression(
                                block: serializeBlock,
                                parameterList: Syntax.ParameterList(
                                    Syntax.Parameter(
                                        identifier: "buffer",
                                        type: Syntax.ParseName("byte[]")
                                    ),
                                    Syntax.Parameter(
                                        identifier: "start",
                                        type: Syntax.ParseName("int")
                                    )
                                )
                            )
                        )) })));



            // var (bytes, iStart, len ) = m_Serializer.Serialize(msg);
            serializeBlock.Statements.Add(Syntax.LocalDeclarationStatement(
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
                                            argumentList: Syntax.ArgumentList(
                                                Syntax.Argument(Syntax.ParseName("msg")),
                                                Syntax.Argument(Syntax.ParseName("buffer")),
                                                Syntax.Argument(Syntax.ParseName("start"))
                                            )
                                        )
                                    )
                    ) }
                )
             ));

            serializeBlock.Statements.Add(Syntax.ReturnStatement(Syntax.ParseName("msgSerializeInfo")));

            // var (byteRet, indexRet, lenRet) = await m_CallAsync.SendWithResponse<ReturnValue>(ChunkType, iCommunicateID, iProtoID, bytes, iStart, len);
            List<ArgumentSyntax> sendMsgArgs = new List<ArgumentSyntax>();
            sendMsgArgs.Add(Syntax.Argument(Syntax.ParseName("ChunkType")));
            if (!bHasReturnValue)
                sendMsgArgs.Add(Syntax.Argument(Syntax.LiteralExpression(0)));
            sendMsgArgs.Add(Syntax.Argument((Syntax.CastExpression("int", Syntax.ParseName("ProtoID." + msgInProtoID.Identifier)))));
            sendMsgArgs.Add(Syntax.Argument(Syntax.ParseName("f")));

            ExpressionSyntax sendExpression = Syntax.InvocationExpression(
                expression: Syntax.ParseName(sendMsgSyntax),
                argumentList: Syntax.ArgumentList( sendMsgArgs.ToArray())
                );

            if (bHasReturnValue)
            {
                sendExpression = Syntax.AwaitExpression(sendExpression);
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

                // return await Task.FromResult((ret.a, ret.a));
                rpcCallMDS.Body.Statements.Add(Syntax.ReturnStatement(Syntax.ParseName("ret.Value")));
            }
            else
            {
                rpcCallMDS.Body.Statements.Add(Syntax.ExpressionStatement(
                    sendExpression
                    ));
            }
            #endregion

            #region 协议处理类

            // 函数名
            MethodDeclarationSyntax handlerMDS = new MethodDeclarationSyntax
            {
                Modifiers = Modifiers.Private,
                Identifier = "Process_" + szMiName,
                ParameterList = Syntax.ParameterList(
                    Syntax.Parameter(type: Syntax.ParseName("int"), identifier: "iCommunicateID"),
                    Syntax.Parameter(type: Syntax.ParseName("byte[]"), identifier: "bytes"),
                    Syntax.Parameter(type: Syntax.ParseName("int"), identifier: "iStartIndex"),
                    Syntax.Parameter(type: Syntax.ParseName("int"), identifier: "iCount")
                    ),
                Body = new BlockSyntax(),
                ReturnType = Syntax.ParseName("void")
            };
            handlerMapClassImplNode.Members.Add(handlerMDS);

            // 注册
            // Add((int)ProtoID.EICHelloService_Hello_MsgIn, Process_ICHelloService_Hello);
            s_HandlerMapConstructorBody.Statements.Add(Syntax.ExpressionStatement(
                Syntax.InvocationExpression(
                    Syntax.ParseName("service.CallAsync.AddProtocolHandler"),
                    Syntax.ArgumentList(
                        Syntax.Argument(
                            Syntax.CastExpression("int", Syntax.ParseName($"{s_EDSProtoID.Identifier}.{msgInProtoID.Identifier}"))),
                        Syntax.Argument(
                            Syntax.ParseName(handlerMDS.Identifier))
                        )
                    )
                ));

            // 解析协议
            //var p = CHelloService.Serializer.Deserialize<Param>(bytes, iStartIndex, iCount);
            handlerMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                        declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName(MsgInStruct.Identifier),
                    new[] {Syntax.VariableDeclarator(
                                    "msg",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.InvocationExpression(
                                            Syntax.ParseName($"m_service.Serializer.Deserialize<{MsgInStruct.Identifier}>"),
                                            argumentList: Syntax.ArgumentList(
                                                Syntax.Argument(Syntax.ParseName(handlerMDS.ParameterList.Parameters[1].Identifier)),
                                                Syntax.Argument(Syntax.ParseName(handlerMDS.ParameterList.Parameters[2].Identifier)),
                                                Syntax.Argument(Syntax.ParseName(handlerMDS.ParameterList.Parameters[3].Identifier))
                                            )
                                        )
                                    )
                          )}
                    )
                ));

            // call logic process function
            // var ret = m_service.Hello2(p);
            ArgumentListSyntax argsList = new ArgumentListSyntax();
            if (aPiIn.Length == 1)
            {
                argsList.Arguments.Add(Syntax.Argument(Syntax.ParseName($"msg.{aPiIn[0].Name}")));
            }
            else
            {
                for (int j = 0; j < aPiIn.Length; j++)
                {
                    argsList.Arguments.Add(Syntax.Argument(Syntax.ParseName($"msg.Value.Item{i}")));
                }
            }

            InvocationExpressionSyntax callLogicProcessFunc =
                Syntax.InvocationExpression(
                                            Syntax.ParseName($"m_service.{szMiName}"),
                                            argumentList: argsList
                                        );

            if (bHasReturnValue)
            {
                handlerMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                        declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "v1",
                                    initializer: Syntax.EqualsValueClause(
                                        callLogicProcessFunc
                                    )
                          )}
                    )
                ));

                handlerMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                        declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "v2",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.InvocationExpression(Syntax.ParseName("v1.GetAwaiter"), new ArgumentListSyntax())
                                    )
                          )}
                    )
                ));


                handlerMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                        declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "ret",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.InvocationExpression(Syntax.ParseName("v2.GetResult"), new ArgumentListSyntax())
                                    )
                          )}
                    )
                ));

                // 序列化协议
                // var ser = CHelloService.Serializer.Serialize(ret);
                handlerMDS.Body.Statements.Add(Syntax.LocalDeclarationStatement(
                        declaration: Syntax.VariableDeclaration(
                    Syntax.ParseName("var"),
                    new[] {Syntax.VariableDeclarator(
                                    "ser",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.InvocationExpression(
                                            Syntax.ParseName($"m_service.Serializer.Serialize"),
                                            argumentList: Syntax.ArgumentList(
                                                Syntax.Argument(Syntax.ParseName("ret"))
                                            )
                                        )
                                    )
                          )}
                    )
                ));

                // return response protocol if need
                // m_service.CallAsync.SendWithoutResponse(iCommunicateID, (int)ProtoID.EICHelloService_Hello3_MsgOut, ser.Item1, ser.Item2, ser.Item3);
                handlerMDS.Body.Statements.Add(Syntax.ExpressionStatement(
                        Syntax.InvocationExpression(
                                            Syntax.ParseName($"m_service.CallAsync.SendWithoutResponse"),
                                            argumentList: Syntax.ArgumentList(
                                                Syntax.Argument(Syntax.ParseName("m_service.ChunkType")),
                                                Syntax.Argument(Syntax.ParseName("iCommunicateID")),
                                                Syntax.Argument(Syntax.CastExpression("int", Syntax.ParseName($"ProtoID.{msgOutProtoID.Identifier}"))),
                                                Syntax.Argument(Syntax.ParseName("ser.Item1")),
                                                Syntax.Argument(Syntax.ParseName("ser.Item2")),
                                                Syntax.Argument(Syntax.ParseName("ser.Item3"))
                                            )
                                        )
                ));
            }
            else
            {
                handlerMDS.Body.Statements.Add(
                    Syntax.ExpressionStatement(callLogicProcessFunc)
                    );
            }

            #endregion



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

            Type argument = typeRet.GenericTypeArguments[0];
            if (argument.IsGenericType)
            {
                if (argument.GetGenericTypeDefinition().ToString().StartsWith("System.ValueTuple"))
                {
                    sb.Append("ValueTuple<");
                    foreach (Type t in typeRet.GenericTypeArguments[0].GenericTypeArguments)
                    {
                        sb.Append(t.ToString()).Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1).Append(">");
                }
                else
                {
                    Console.WriteLine("Task<T>, T Must ValueTuple or Non-Generic");
                    return "";
                }
            }
            else
            {
                sb.Append(argument.ToString());
            }

            return sb.ToString();
        }
    }
}
