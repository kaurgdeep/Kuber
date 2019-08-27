using Microsoft.EntityFrameworkCore;
using KuberAPI.Interfaces.Services;
using KuberAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Services
{
    public class RideService : IEntityService<Ride>
    {
        private KuberContext Context;
        public RideService(KuberContext context)
        {
            Context = context;
        }

        public int Create(Ride ride)
        {
            Context.Rides.Add(ride);
            Context.SaveChanges();

            return ride.RideId;
        }

        public Ride Get(Func<Ride, bool> filter)
        {
            return Context.Rides
                .Include(_ => _.FromAddress)
                .Include(_ => _.ToAddress)
                .Where(filter).FirstOrDefault();
        }

        public int Count(Func<Ride, bool> filter)
        {
            return Context.Rides.Where(filter).Count();
        }

        public IEnumerable<Ride> GetMany(Func<Ride, bool> filter)
        {
            return Context.Rides
                .Include(_ => _.FromAddress)
                .Include(_ => _.ToAddress)
                .Where(filter);
        }

        public Ride Update(Action updateFn, Ride ride)
        {
            updateFn?.Invoke();
            Context.SaveChanges();

            return ride;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
