using Bogus;

namespace FC.Codeflix.Catalog.Tests.Shared;
public abstract class DataGeneratorBase
{
    protected DataGeneratorBase()
    {
        Faker = new Faker("pt_BR");
    }

    public Faker Faker { get; set; }

    public bool GetRandomBoolean() => new Random().NextDouble() <= 0.5;
}
