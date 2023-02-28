using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

public class CustomerRepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCreateCustomer()
    {
        var repository = new CustomerRepository();
        Assert.NotNull(repository);
    }
}