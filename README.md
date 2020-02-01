| Projekt | Status |
| --- | --- |
| TreeLine.Sagas | [![Build Status](https://serowy.visualstudio.com/TreeLine.Sagas/_apis/build/status/release%20-%20sagas?branchName=release%2F1.1)](https://serowy.visualstudio.com/TreeLine.Sagas/_build/latest?definitionId=6&branchName=release%2F1.1) | 
| TreeLine.Sagas.DependencyInjection | [![Build Status](https://serowy.visualstudio.com/TreeLine.Sagas/_apis/build/status/release%20-%20sagas_dependency%20injection?branchName=release%2F1.1)](https://serowy.visualstudio.com/TreeLine.Sagas/_build/latest?definitionId=7&branchName=release%2F1.1) |

# TreeLine.Sagas
## What is this package about?
In distributed systems, such as microservices, business processes pose multiple challenges. There are different ways of solving these challenges, for example, to counter rollback scenarios or changes in business processes.
One of these approaches is the design pattern Saga-Orchestration.

## How to use it?
Three configureation steps are necessary to use the package.

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
To register an own implementation of ISagaEventStore you should use the given overload to register the pakage.
```csharp
IServiceCollection AddSagas<TEventStore>(this IServiceCollection services) where TEventStore : class, ISagaEventStore
```
### 4. Get a Saga
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