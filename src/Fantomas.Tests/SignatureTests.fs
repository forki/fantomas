﻿module Fantomas.Tests.SignatureTests

open NUnit.Framework
open FsUnit

open Fantomas.CodeFormatter
open Fantomas.Tests.TestHelper

// the current behavior results in a compile error since "(string * string) list" is converted to "string * string list"
[<Test>]
let ``should keep the (string * string) list type signature in records``() =
    formatSourceString false """type MSBuildParams = 
    { Targets : string list
      Properties : (string * string) list
      MaxCpuCount : int option option
      ToolsVersion : string option
      Verbosity : MSBuildVerbosity option
      FileLoggers : MSBuildFileLoggerConfig list option }

    """ config
    |> should equal """type MSBuildParams = 
    { Targets : string list;
      Properties : (string * string) list;
      MaxCpuCount : int option option;
      ToolsVersion : string option;
      Verbosity : MSBuildVerbosity option;
      FileLoggers : MSBuildFileLoggerConfig list option }
"""

[<Test>]
let ``should keep the (string * string) list type signature in functions``() =
    formatSourceString false """let MSBuildWithProjectProperties outputPath (targets : string) 
    (properties : string -> (string * string) list) projects = doingsomstuff

    """ config
    |> should equal """let MSBuildWithProjectProperties outputPath (targets : string) 
    (properties : string -> (string * string) list) projects = doingsomstuff
"""


[<Test>]
let ``should keep the string * string list type signature in functions``() =
    formatSourceString false """let MSBuildWithProjectProperties outputPath (targets : string) 
    (properties : (string -> string) * string list) projects = doingsomstuff

    """ config
    |> should equal """let MSBuildWithProjectProperties outputPath (targets : string) 
    (properties : (string -> string) * string list) projects = doingsomstuff
"""

[<Test>]
let ``should not add parens in signature``() =
    formatSourceString false """type Route = 
    { Verb : string
      Path : string
      Handler : Map<string, string> -> HttpListenerContext -> string }
    override x.ToString() = sprintf "%s %s" x.Verb x.Path

    """ config
    |> should equal """type Route = 
    { Verb : string;
      Path : string;
      Handler : Map<string, string> -> HttpListenerContext -> string }
    override x.ToString() = sprintf "%s %s" x.Verb x.Path
"""

[<Test>]
let ``should keep the string * string * string option type signature``() =
    formatSourceString false """type DGML = 
    | Node of string
    | Link of string * string * (string option)

    """ config
    |> should equal """type DGML = 
    | Node of string
    | Link of string * string * string option
"""

[<Test>]
let ``should keep the (string option * Node) list type signature``() =
    formatSourceString false """type Node = 
    { Name : string;
      NextNodes : (string option * Node) list }

    """ config
    |> should equal """type Node = 
    { Name : string;
      NextNodes : (string option * Node) list }
"""
