﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DS;
using DO;

namespace Dal
{
    sealed class DalObject : IDal
    {
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use


        #endregion

        #region Bus
        public void CreateBus(Bus bus)
        {
            DataSource.Buses.Add(bus);
        }

        public void DeleteBus(int id)
        {
            var bus = DataSource.Buses.Find((Bus) => { return Bus.Id == id; });
            if (bus.Id != id)
                throw new BusExceptionDO(id, "system not found the bus");
            if (bus.Active == false)
                throw new BusExceptionDO(id, "the bus is already not Active");
            bus.Active = false;
        }

        public void UpdateBus(Bus newBus)
        {
            int index = DataSource.Buses.FindIndex((Bus) => { return Bus.Id == newBus.Id; });
            DataSource.Buses[index] = newBus;
        }

        public Bus GetBus(int id)
        {
            var bus = DataSource.Buses.Find((Bus) => { return Bus.Id == id; });
            if (bus.Id != id)
                throw new BusExceptionDO(id, "system not found the bus");
            if (bus.Active == false)
                throw new BusExceptionDO(id, "the bus is not Active");
            return bus.Clone();
        }

        public IEnumerable<Bus> GetBuses()
        {
            return from Bus in DataSource.Buses
                   where Bus.Active == true
                   select Bus.Clone();
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            return from Bus in DataSource.Buses
                   where predicate(Bus)
                   where Bus.Active == true
                   select Bus.Clone();
        }

        #endregion

        public User GetUser(string userName)
        {
            var user = DataSource.Users.Find((User) => { return User.UserName == userName; });
            if (user.UserName != userName)
                throw new UserExceptionDO(userName, "system not found the userName");
            if (user.Active == false)
                throw new UserExceptionDO(userName, "userName is not Active");
            return user.Clone();
        }
        public void addUser(User user)
        {
            DataSource.Users.Add(user);
        }
        public void deleteUser(string phone, DateTime dateTime)
        {
            var user = DataSource.Users.Find((User) => { return (User.Phone == phone && User.Birthday == dateTime); });
            if (user.Phone != phone)
            {
                throw new UserExceptionDO(phone, "system not found the userName");
            }
            if(user.Active==false)
                throw new UserExceptionDO(phone, "userName is already not Active");
            user.Active = false;
        }

        public IEnumerable<Driver> GetDrivers()
        {
            return from Driver in DataSource.Drivers
                   where Driver.Active == true
                   select Driver.Clone();
        }

        public IEnumerable<Driver> GetDriversBy(Predicate<Driver> predicate)
        {
            return from Driver in DataSource.Drivers
                   where predicate(Driver)
                   where Driver.Active == true
                   select Driver.Clone();
        }

        public Driver GetDriver()
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(Driver newDriver)
        {
            var oldDriver = DataSource.Drivers.Find((Driver) => { return Driver.Id == newDriver.Id; });
            oldDriver = newDriver;
        }

        public void DeleteDriver(int id)
        {
            throw new NotImplementedException();
        }

        public void updateUser(User user)
        {
            int index = DataSource.Users.FindIndex((User) => { return User.UserName == user.UserName; });
            DataSource.Users[index] = user;
        }
    }
}
