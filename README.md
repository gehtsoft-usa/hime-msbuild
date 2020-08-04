# What is it about?

MSBuild Task to compile Hime's grammar files

Hime is grammar compile provided under LGPL license by [Cénotélie](https://github.com/cenotelie/hime-grams)

The default option to compile grammar files is to use console compiler which is somehow integrated to your build process. 

This solution is an alternative offering "native" msbuild task to compile grammars. 

# How to use?

1) Add task package to your csproj file:
```
    <ItemGroup>
      ...
      &lt;PackageReference Include="Hime.Build.Task" Version="0.1.6" IncludeAssets="build" /&gt;
    &lt;/ItemGroup&gt;
```
2) Add the target to compile grammar before source code compilation
    <Target Name="CompileGrammar" BeforeTargets="BeforeCompile">
     <CompileGrammar GrammarName="MyGrammar" 
                     GrammarFile="MyGrammar.gram" 
                     Namespace="MyParser.Grammar" 
                     OutputMode="Source" 
                     CodeAccess="Internal" 
                     OutputPath="." />
    </Target>

3) Add the target to clean up the compilation result (optional)
    <Target Name="CleanGrammar" BeforeTargets="Clean">
      <Delete Files="MyGrammarLexer.bin" />
      <Delete Files="MyGrammarParser.bin" />
      <Delete Files="MyGrammarParser.cs" />
      <Delete Files="MyGrammarLexer.cs" />
    </Target>

4) Switch your project to explicit compilation
    <PropertyGroup>
      ...
      <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    </PropertyGroup>

5) Add your files including auto-generated files to compilation
    <ItemGroup>
      <!--Autogenerated -->
      <EmbeddedResource Include="MyGrammarLexer.bin" />
      <EmbeddedResource Include="MyGrammarParser.bin" />
      <Compile Include="MyGrammarLexer.cs" />
      <Compile Include="MyGrammarParser.cs" />
      <!--Source code -->
      <Compile Include="MyCompiler.cs" />
    </ItemGroup>

6) That's all, it should work both in msbuild console compilator as well as under Visual Studio 2017+
