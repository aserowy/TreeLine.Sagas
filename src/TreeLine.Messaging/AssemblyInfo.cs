using System.Runtime.CompilerServices;

// To enable moq to create mocks for internals
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

[assembly: InternalsVisibleTo("TreeLine.Messaging.DependencyInjection")]
[assembly: InternalsVisibleTo("TreeLine.Messaging.Tests")]