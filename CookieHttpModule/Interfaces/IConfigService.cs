

namespace Example.Namespace.HttpModule.Interfaces
{
    public interface IConfigService
    {
        string ExampleApiKey { get; }
        string[] KeysToWatchFor { get; }
    }
}
