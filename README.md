[![Build Status](https://serowy.visualstudio.com/TreeLine.Sagas/_apis/build/status/release?branchName=release%2F1.2)](https://serowy.visualstudio.com/TreeLine.Sagas/_build/latest?definitionId=9&branchName=release%2F1.2)

| Project | Package |
| --- | --- |
| TreeLine.Sagas | [NuGet](https://www.nuget.org/packages/TreeLine.Sagas/) | 
| TreeLine.Sagas.DependencyInjection | [NuGet](https://www.nuget.org/packages/TreeLine.Sagas.DependencyInjection/) | 
| TreeLine.Sagas.Validation | [NuGet](https://www.nuget.org/packages/TreeLine.Sagas.Validation/) | 

# TreeLine.Sagas
## What is this package about?
In distributed systems, such as microservices, business processes pose multiple challenges. There are different ways of solving these challenges, for example, to counter rollback scenarios or changes in business processes.
One of these approaches is the design pattern Saga-Orchestration.

## Before we start
This whole package revolves around ISagaEvent and ISagaCommand. Events are triggers for certain steps in your saga. So a concrete step of your business logic is called. Commands on the other hand will leave your saga to trigger operations on e.g. another service. This service will communicate the results with an event. To reference a saga or a specific transaction of a saga, both interfaces implement two simple properties.
```csharp
public interface ISagaEvent
{
    Guid ProcessId { get; }
    Guid TransactionId { get; }
}
```
Both Ids deal with different scopes. The ProcessId is used to reference an entire saga run. Therefore, events and commands should reuse this Id to allow references to the complete business process of a saga.
The TransactionId handles a smaller scope. It allows references from one command to n events. Each command must have a unique TransactionId and each event created by that command should reuse that Id.

## How to use it?
### 1. Build steps of your Saga
The first step is to develop the individual steps to define a business process. Since sagas consist of individual messages that in turn trigger parts of the business process, a step consists of a trigger (message type) and the corresponding logic.
```csharp
public sealed class SagaStepMock : ISagaStep<SagaEvent>
{
    public Task<IEnumerable<ISagaCommand>> RunAsync(SagaEvent sagaEvent)
    {
        return Task.FromResult(new[] { new SagaCommand() }.AsEnumerable<ISagaCommand>());
    }
}
```
It is important that the used messages inherit from type ISagaEvent.

### 2. Build your Saga with concrete steps
In the second step the developed ISagaStep<> are versioned and combined. It is important that the assigned version complies with the convention of "semantic version".
```csharp
private sealed class SagaProfileMock : ISagaProfile
{
    public void Configure(ISagaProcessorBuilder processorBuilder)
    {
        processorBuilder
            .AddVersion("1.0.0")
            .AddStep<SagaEvent, SagaStepMock>();
    }
}
```
As soon as minor or major of the version has to be incremented, a new version is created in the saga definition. This ensures that already running business processes are still handled with the old version of the saga.
As long as only the patch level is raised, the old saga definition is still used.
```csharp
public sealed class SagaProfileMock : ISagaProfile
{
    public void Configure(ISagaProcessorBuilder processorBuilder)
    {
        processorBuilder
            .AddVersion("1.0.12")
            .AddStep<SagaEvent, SagaStepMock>();

        processorBuilder
            .AddVersion("1.1.0")
            .AddStep<SagaEvent, SagaStepMock>();
    }
}
```
To support backwards compatibility, an own implementation of the interface ISagaEventStore must be registered.
### 3. Register sagas with dependency injection (MS package)
Finally, all implementations must be registered. It is important that "Microsoft.Extensions.Logging" is registered as well.
```csharp
var services = new ServiceCollection()
    .AddSagas()
    .AddLogging()
    .AddTransient<SagaProfileMock>()
    .AddTransient<SagaStepMock>();
```
To register an own implementation of ISagaEventStore you should use the given overload to register the package.
```csharp
var services = new ServiceCollection()
    .AddSagas(bldr => bldr.AddEventStore<SagaEventStoreMock>())
    .AddLogging()
    .AddTransient<SagaProfileMock>()
    .AddTransient<SagaStepMock>();
```
### 4. Validate Sagas
By validating your sagas you can ensure that most failures of your profiles are checked. If you have for example at least one saga profile with two identical version identifier the programm will throw an exception at start. To validate your profiles you should use the extension on IServiceProvider on startup.
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app
        .ApplicationServices
        .ValidateSagas();
```
### 5. Get a Saga
Now the different sagas can be created and used via a factory.
```csharp
var saga = services
    .BuildServiceProvider()
    .GetRequiredService<ISagaFactory>()
    .Create<SagaProfileMock>();

var commands = await saga
    .RunAsync(new SagaEvent())
    .ConfigureAwait(false);
```
The saga creates ISagaCommand objects, which must be sent independently over the directed channel. This must not be part of the Saga implementation.
