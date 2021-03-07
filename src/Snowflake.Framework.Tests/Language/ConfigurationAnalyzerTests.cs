using Snowflake.Language.Analyzers.Configuration;
using Snowflake.Language.Tests;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Language.Tests
{
    public class ConfigurationAnalyzerTests
    {
        [Theory]
        [InlineData("SectionTest.SFC001.Test.cs", "SectionTest.SFC001.Fix.cs", 5, 5)]
        [InlineData("CollectionTest.SFC001.Test.cs", "CollectionTest.SFC001.Fix.cs", 5, 5)]
        [InlineData("InputConfigTest.SFC001.Test.cs", "InputConfigTest.SFC001.Fix.cs", 5, 5)]

        public async Task SFC001_UnextendibleInterfaceFix_Test(string testCodePath, string fixedCodePath, int line, int col)
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC001.{testCodePath}");
            string fixedCode = TestUtilities.GetStringResource($"Language.SFC001.{fixedCodePath}");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <UnextendibleInterfaceAnalyzer, UnextendibleInterfaceFix>
             (testCode, fixedCode, (line, col), "TestInterface");

            await harness.RunAsync();
        }

        [Theory]
        [InlineData("SectionTest.SFC002.Test.cs", 7, 9)]
        [InlineData("CollectionTest.SFC002.Test.cs", 7, 9)]
        [InlineData("InputConfigTest.SFC002.Test.cs",  7, 9)]
        public async Task SFC002_TemplateInterfaceTopLevel_Test(string testCodePath, int line, int col)
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC002.{testCodePath}");
            var harness = LanguageTestUtilities.MakeAnalyzerTest<TemplateInterfaceTopLevelAnalyzer>
                (testCode, (line, col), "TestInterface");

            await harness.RunAsync();
        }

        [Theory]
        [InlineData("CollectionTest.SFC003.Test.cs", "CollectionTest.SFC003.Fix.cs", "Method", "BadMember", 8, 14)]
        [InlineData("SectionTest.SFC003.Test.cs", "SectionTest.SFC003.Fix.cs", "Method", "BadMember", 8, 14)]
        [InlineData("InputConfigurationTest.SFC003.Test.cs", "InputConfigurationTest.SFC003.Fix.cs", "Property", "this[]", 8, 16)]
        public async Task SFC003_InvalidTemplateMember_Test(string testCodePath, string fixedCodePath, string type, string member, int line, int col)
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC003.{testCodePath}");
            string fixedCode = TestUtilities.GetStringResource($"Language.SFC003.{fixedCodePath}");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <InvalidTemplateMemberAnalyzer, InvalidTemplateMemberFix>
             (testCode, fixedCode, (line, col), type, "TestInterface", member);

            await harness.RunAsync();
        }


        [Theory]
        [InlineData("CollectionTest.SFC004.Test.cs", "CollectionTest.SFC004.Fix.cs", 14, 16)]
        [InlineData("SectionTest.SFC004.Test.cs", "SectionTest.SFC004.Fix.cs", 14, 16)]
        [InlineData("InputConfigurationTest.SFC004.Test.cs", "InputConfigurationTest.SFC004.Fix.cs", 14, 16)]
        public async Task SFC004_CannotHideInheritedProperty_Test(string testCodePath, string fixedCodePath, int line, int col)
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC004.{testCodePath}");
            string fixedCode = TestUtilities.GetStringResource($"Language.SFC004.{fixedCodePath}");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <CannotHideInheritedPropertyAnalyzer, CannotHideInheritedPropertyFix>
             (testCode, fixedCode, (line, col), "InheritedProperty", "TestInterface");

            await harness.RunAsync();
        }

        [Theory]
        [InlineData("CollectionTest.SFC005.Test.cs", "CollectionTest.SFC005.Fix.cs", new int[] { 8, 8 }, new int[] { 36, 55 }, new string[] { "get", "set"})]
        [InlineData("SectionTest.SFC005.Test.cs", "SectionTest.SFC005.Fix.cs", new int[] { 8 }, new int[] { 34 }, new string[] { "get" })]
        [InlineData("InputConfigurationTest.SFC005.Test.cs", "InputConfigurationTest.SFC005.Fix.cs", new int[] { 8 }, new int[] { 36 }, new string[] { "get" })]
        public async Task SFC005_InvalidTemplateAccessor_Test(string testCodePath, string fixedCodePath, int[] lines, int[] cols, string[] accessors)
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC005.{testCodePath}");
            string fixedCode = TestUtilities.GetStringResource($"Language.SFC005.{fixedCodePath}");

            var args = new List<(int, int, object[])>();
            for (int i = 0; i < lines.Length; i++)
            {
                args.Add((lines[i], cols[i], new[] { "InheritedProperty", accessors[i] }));
            }
            var harness = LanguageTestUtilities.MakeCodeFixTest
                <InvalidTemplateAccessorAnalyzer, InvalidTemplateAccessorFix>
             (testCode, fixedCode, args.ToArray());

            await harness.RunAsync();
        }
    }
}