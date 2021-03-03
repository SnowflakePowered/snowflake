; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
SFC001 | Snowflake.Configuration | Error | UnextendibleInterfaceAnalyzer
SFC002 | Snowflake.Configuration | Error | TemplateInterfaceTopLevelAnalyzer
SFC003 | Snowflake.Configuration | Error | InvalidTemplateMember
SFC004 | Snowflake.Configuration | Error | CannotHideInheritedPropertyAnalyzer
SFC005 | Snowflake.Configuration | Error | CollectionPropertiesNotInterfaceAnalyzer
SFC006 | Snowflake.Configuration | Error | CollectionPropertiesNotInterfaceAnalyzer
SFC007 | Snowflake.Configuration | Error | CollectionPropertyInvalidAccessorAnalyzer
SFC008 | Snowflake.Configuration | Error | CollectionPropertyMustHaveGetterAnalyzer
SFC009 | Snowflake.Configuration | Error | DuplicateConfigurationTargetAnalyzer
SFC010 | Snowflake.Configuration | Error | UndecoratedSectionPropertyAnalyzer
SFC011 | Snowflake.Configuration | Error | SectionPropertyMismatchedTypeAnalyzer
SFC012 | Snowflake.Configuration | Error | SectionPropertyEnumUndecorated
SFC013 | Snowflake.Configuration | Error | CollectionPropertyInvalidAccessorAnalyzer
SFC014 | Snowflake.Configuration | Error | SectionPropertyMustHaveGetterAnalyzer
SFC015 | Snowflake.Configuration | Error | SectionPropertyMustHaveSetterAnalyzer
SFC016 | Snowflake.Configuration | Error | InputPropertyInvalidAccessorAnalyzer
SFC017 | Snowflake.Configuration | Error | InputPropertyUndecoratedAnalyzer
SFC018 | Snowflake.Configuration | Error | InputPropertyTypeMismatchAnalyzer
SFC019 | Snowflake.Configuration | Error | OnlyOneAttributeTypeAnalyzer
SFC020 | Snowflake.Configuration | Error | OnlyOneAttributePropertyTypeAnalyzer
SFC021 | Snowflake.Configuration | Error | GenericArgumentRequiresConfigurationCollection
SFG000 | Snowflake.Language | Error | DiagnosticReporting