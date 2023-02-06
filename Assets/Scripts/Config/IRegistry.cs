using System.Collections;

// ReSharper disable once CheckNamespace
namespace Config
{
    public interface IRegistry
    {
    }

    public interface IRegistryClass : IRegistry
    {
    }

    public interface IRegistryList : IEnumerable, IRegistry
    {
    }

    public interface IRegistryData
    {
        string Id { get; }
    }
}