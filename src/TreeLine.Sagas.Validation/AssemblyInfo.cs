using System.Runtime.CompilerServices;

// To enable moq to create mocks for internals
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

[assembly: InternalsVisibleTo("TreeLine.Sagas.DependencyInjection")]
[assembly: InternalsVisibleTo("TreeLine.Sagas.Validation.Tests")]