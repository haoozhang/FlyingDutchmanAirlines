using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

public class CustomerRepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestCreateCustomer_Success()
    {
        var repository = new CustomerRepository();
        Assert.IsNotNull(repository);

        var result = await repository.CreateCustomer("test-name");
        Assert.IsTrue(result);
    }
    
    [Test]
    public async Task TestCreateCustomer_Failure_NameNullOrEmpty()
    {
        var repository = new CustomerRepository();
        Assert.IsNotNull(repository);

        var result = await repository.CreateCustomer("");
        Assert.IsFalse(result);

        result = await repository.CreateCustomer(null);
        Assert.IsFalse(result);
    }
    
    // inline test data
    [TestCase('!')]
    [TestCase('@')]
    [TestCase('#')]
    [TestCase('$')]
    [TestCase('%')]
    [TestCase('&')]
    [TestCase('*')]
    public async Task TestCreateCustomer_Failure_NameInvalid(char invalidChar)
    {
        var repository = new CustomerRepository();
        Assert.IsNotNull(repository);

        var result = await repository.CreateCustomer("test-name" + invalidChar);
        Assert.IsFalse(result);
    }
}