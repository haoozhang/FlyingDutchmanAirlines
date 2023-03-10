using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

public class CustomerRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;

    private CustomerRepository _repository;
    
    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchmanAirlines").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        _repository = new CustomerRepository(_context);
        Assert.IsNotNull(_repository);
    }
    
    [Test]
    public async Task TestCreateCustomer_Success()
    {
        var result = await _repository.CreateCustomer("test-name");
        Assert.IsTrue(result);

        await _context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task TestCreateCustomer_Failure_NameNullOrEmpty()
    {
        try
        {
            await _repository.CreateCustomer("");
            Assert.Fail("Argument exception should be thrown.");
        }
        catch (ArgumentException e)
        {
            Assert.Pass("Argument exception expected.");
        }
        
        try
        {
            await _repository.CreateCustomer(null);
            Assert.Fail("Argument exception should be thrown.");
        }
        catch (ArgumentException e)
        {
            Assert.Pass("Argument exception expected.");
        }
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
        try
        {
            await _repository.CreateCustomer("test-name" + invalidChar);
            Assert.Fail("Argument exception should be thrown.");
        }
        catch (ArgumentException e)
        {
            Assert.Pass("Argument exception expected.");
        }
    }

    [Test]
    public async Task TestGetCustomerByName_Success()
    {
        var customerName = "test-name";
        var result = await _repository.CreateCustomer(customerName);
        Assert.IsTrue(result);

        var dbCustomer = _context.Customers.First();
        var customer = await _repository.GetCustomerByName(customerName);
        Assert.IsNotNull(customer);
        Assert.That(customer, Is.EqualTo(dbCustomer));
    }
    
    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}