using KuberAPI.Interfaces.Services;
using KuberAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Services
{
    public class AddressService : IEntityService<Address>
    {
        private KuberContext Context;
        public AddressService(KuberContext context)
        {
            Context = context;
        }

        public int Create(Address address)
        {
            Context.Addresses.Add(address);
            Context.SaveChanges();

            return address.AddressId;
        }

        public Address Get(Func<Address, bool> filter)
        {
            return Context.Addresses.Where(filter).FirstOrDefault();
        }

        public int Count(Func<Address, bool> filter)
        {
            return Context.Addresses.Where(filter).Count();
        }

        public IEnumerable<Address> GetMany(Func<Address, bool> filter)
        {
            return Context.Addresses.Where(filter);
        }

        public Address Update(Action updateFn, Address address)
        {
            updateFn?.Invoke();
            Context.SaveChanges();

            return address;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
