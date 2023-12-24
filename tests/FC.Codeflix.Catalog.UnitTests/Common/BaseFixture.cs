namespace FC.Codeflix.Catalog.UnitTests.Common;
public abstract class BaseFixture
{
    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

    public Faker Faker { get; set; }

    public bool GetRandomBoolean() => new Random().NextDouble() <= 0.5;
}
