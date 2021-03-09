using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Snowflake.Language.Generators.Configuration;
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
    public class ConfigurationGeneratorTests
    {
        [Fact]
        public async Task ConfigurationSection_Generator_Test()
        {
            string source = TestUtilities.GetStringResource("Language.Generators.Configuration.GeneratorTestExampleConfigurationSection.cs");
            var compilation = await LanguageTestUtilities.CreateCompilation(source);
            var generator = new ConfigurationSectionGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
            Assert.Empty(outputCompilation.GetDiagnostics());
            Assert.Equal(2, outputCompilation.SyntaxTrees.Count());
        }

        [Fact]
        public async Task ConfigurationCollection_Generator_Test()
        {
            string source = TestUtilities.GetStringResource("Language.Generators.Configuration.GeneratorTestExampleConfigurationCollection.cs");
            var compilation = await LanguageTestUtilities.CreateCompilation(source);
            var generator = new ConfigurationCollectionGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
            Assert.Empty(outputCompilation.GetDiagnostics());
            Assert.Equal(2, outputCompilation.SyntaxTrees.Count());
        }

        [Fact]
        public async Task InputConfiguration_Generator_Test()
        {
            string source = TestUtilities.GetStringResource("Language.Generators.Configuration.GeneratorTestRetroArchInput.cs");
            var compilation = await LanguageTestUtilities.CreateCompilation(source);
            var generator = new InputConfigurationGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
            Assert.Empty(outputCompilation.GetDiagnostics());
            Assert.Equal(2, outputCompilation.SyntaxTrees.Count());
        }
    }
}
