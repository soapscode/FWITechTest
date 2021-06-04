using System;
using System.Collections.Generic;
using System.Linq;
using TechTestLib.Interfaces;

namespace Interview
{
    public class CustomerRepository : IRepository<ICustomer>
    {
        List<ICustomer> _customers;

        public CustomerRepository(IEnumerable<ICustomer> customers)
        {
            _customers = customers.ToList();
        }

        public IEnumerable<ICustomer> All()
        {
            return _customers;
        }

        public void Delete(IComparable id)
        {
            if (ReferenceEquals(null, id))
            {
                throw new ArgumentNullException("id", "");
            }

            if ((int)id <= 0)
            {
                throw new ArgumentOutOfRangeException("No Id provided");
            }

            if (ReferenceEquals(null, _customers))
            {
                throw new NullReferenceException("Customers collection not initialised");
            }

            if (_customers.Count == 0)
            {
                throw new InvalidOperationException("No Customers saved");
            }

            if (ReferenceEquals(null, FindById(id)))
            {
                throw new InvalidOperationException("Customer does not exist");

            }

            _customers.RemoveAll(c => c.Id.Equals(id));
        }

        public void Save(ICustomer item)
        {
            if (ReferenceEquals(null, item))
            {
                throw new ArgumentNullException("item", "");
            }

            if (ReferenceEquals(null, item.Id) || (int)item.Id < 1 )
            {
                throw new ArgumentOutOfRangeException("No Customer data provided");
            }

            if (ReferenceEquals(null, _customers))
            {
                throw new NullReferenceException("Customers collection not initialised");
            }

            if (FindById(item.Id) != null)
            {
                throw new InvalidOperationException("Customer already exists");

            }

            _customers.Add(item);
        }

        public ICustomer FindById(IComparable id)
        {
            if (ReferenceEquals(null, id))
            {
                throw new ArgumentNullException("id", "");
            }

            if ((int)id <= 0)
            {
                throw new ArgumentOutOfRangeException("No Id provided");
            }

            ICustomer foundCustomer = null;

            if (ReferenceEquals(null, _customers))
            {
                throw new NullReferenceException("Customers collection not initialised");
            }

            foundCustomer = _customers.FirstOrDefault(c => c.Id.Equals(id));

            return foundCustomer;
        }

    }
}
