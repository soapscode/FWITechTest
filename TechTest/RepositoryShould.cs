using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTestLib.Interfaces;

namespace Interview
{
    [TestFixture]
    public class RepositoryShould
    {
        private List<Customer> customers;
        Mock<ICustomers> mockCustomers;

        [SetUp]
        public void Setup()
        {
            customers = new List<Customer>();
            mockCustomers = new Mock<ICustomers>();
        }

        [Test]
        public void ReturnNoRecordsFromRepositoryWhenNoRecordsPresent()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());

            var sut = new CustomerRepository(mockCustomers.Object);
            Assert.That(sut.All(), Has.Exactly(0).Items);
        }

        [Test]
        public void ReturnAllRecordsFromRepositoryWhenRecordsPresent()
        {
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            var secondCustomer = new Customer { Id = 2, FirstName = "John", Surname = "Smith" };
            customers.Add(firstCustomer);
            customers.Add(secondCustomer);
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());

            var sut = new CustomerRepository(mockCustomers.Object);
            
            Assert.That(sut.All(), Has.Exactly(2).Items);

            sut.Delete(1);

            Assert.That(sut.All(), Has.Exactly(1).Items);

            sut.Save(firstCustomer);

            Assert.That(sut.All(), Has.Exactly(2).Items);

            Assert.That(sut.All(), Does.Contain(firstCustomer));
            Assert.That(sut.All(), Does.Contain(secondCustomer));
        }

        [Test]
        public void ReturnNoRecordsFromRepositoryWhenAllRecordsHaveBeenDeleted()
        {
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            var secondCustomer = new Customer { Id = 2, FirstName = "John", Surname = "Smith" };
            customers.Add(firstCustomer);
            customers.Add(secondCustomer);
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(sut.All(), Has.Exactly(2).Items);
            Assert.That(sut.All(), Does.Contain(firstCustomer));
            Assert.That(sut.All(), Does.Contain(secondCustomer));

            sut.Delete(1);
            sut.Delete(2);

            Assert.That(sut.All(), Has.Exactly(0).Items);
            Assert.That(sut.All(), Does.Not.Contain(firstCustomer));
            Assert.That(sut.All(), Does.Not.Contain(secondCustomer));
        }

        [Test]
        public void ThrowExceptionWhenDeleteAttemptedButNoIdProvided()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(() => sut.Delete(null), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => sut.Delete(0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ThrowExceptionWhenCollectionNotInitialised()
        {
            Mock<IEnumerable<ICustomer>> mockCustomers = null;
            Assert.That(() => new CustomerRepository(mockCustomers.Object), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ReturnErrorWhenNoRecordsToDelete()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(() => sut.Delete(1), Throws.TypeOf<InvalidOperationException>()
                              .With
                              .Message
                              .EqualTo("No Customers saved"));
        }

        [Test]
        public void ThrowExceptionWhenDeleteAttemptedAndRecordDoesNotExist()
        {
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            customers.Add(firstCustomer);
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(() => sut.Delete(2), Throws.TypeOf<InvalidOperationException>()
                              .With
                              .Message
                              .EqualTo("Customer does not exist"));
        }

        [Test]
        public void ReturnCorrectNumberOfRecordsWhenRecordHasBeenDeleted()
        {
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            var secondCustomer = new Customer { Id = 2, FirstName = "John", Surname = "Smith" };
            customers.Add(firstCustomer);
            customers.Add(secondCustomer);
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(sut.All(), Has.Exactly(2).Items);
            Assert.That(sut.All(), Does.Contain(firstCustomer));

            sut.Delete(1);

            Assert.That(sut.All(), Does.Not.Contain(firstCustomer));
            Assert.That(sut.All(), Does.Contain(secondCustomer));
            Assert.That(sut.All(), Has.Exactly(1).Items);
        }

        [Test]
        public void ThrowExceptionWhenSaveAttemptedButNoDataProvided()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(() => sut.Save(null), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => sut.Save(new Customer()), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ThrowExceptionWhenSaveAttemptedButIdAlreadyExists()
        {
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            var secondCustomer = new Customer { Id = 1, FirstName = "John", Surname = "Smith" };
            customers.Add(firstCustomer);
            customers.Add(secondCustomer);
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(() => sut.Save(secondCustomer), Throws.TypeOf<InvalidOperationException>()
                               .With
                               .Message
                               .EqualTo("Customer already exists"));
        }

        [Test]
        public void ReturnCorrectNumberOfRecordsWhenRecordHasBeenSaved()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            var secondCustomer = new Customer { Id = 2, FirstName = "John", Surname = "Smith" };

            sut.Save(firstCustomer);
            sut.Save(secondCustomer);

            Assert.That(sut.All(), Has.Exactly(2).Items);
            Assert.That(sut.All(), Does.Contain(firstCustomer));
            Assert.That(sut.All(), Does.Contain(secondCustomer));
        }

        [Test]
        public void ThrowExceptionWhenFindAttemptedButNoDataProvided()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);

            Assert.That(() => sut.FindById(null), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => sut.FindById(0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReturnNullWhenRecordNotFoundInRepository()
        {
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);
            var customer = sut.FindById(1);

            Assert.That(customer, Is.Null);
        }

        [Test]
        public void ReturnRecordWhenRecordFoundInRepository()
        {
            var firstCustomer = new Customer { Id = 1, FirstName = "Joe", Surname = "Bloggs" };
            customers.Add(firstCustomer);
            mockCustomers.Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());
            var sut = new CustomerRepository(mockCustomers.Object);
            var customerRetrieved = (Customer)sut.FindById(1);

            Assert.That(firstCustomer, Is.SameAs(customerRetrieved));
        }
    }
}