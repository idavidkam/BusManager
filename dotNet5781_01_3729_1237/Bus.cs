﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_3729_1237
{
    /// <summary>
    /// The class represents a single bus
    /// with functionality to monitor its integrity and suitability for travel
    /// </summary>
    public class Bus
    {
        uint id;
        uint mileage;
        int fuel;
        uint lastCareMileage;
        DateTime dateRoadAscent;
        DateTime lastCare;

        /// <summary>
        /// Represents the bus license number
        ///  by years as provided by law
        /// </summary>
        public uint Id { get => id; private set => id = value; }

        /// <summary>
        /// Displays the date of ascent to the road.
        /// </summary>
        public DateTime DateRoadAscent
        { get => dateRoadAscent; private set => dateRoadAscent = value; }

        /// <summary>
        /// Shows the amount of mileage that the bus passed from entering the Road
        /// </summary>
        public uint Mileage
        {
            get => mileage;
            set
            {
                if (!(mileage >= value))
                    mileage = value;
            }
        }
        /// <summary>
        /// Shows how many miles the bus can travel further
        /// </summary>
        public int Fuel { get => fuel; set => fuel = value; }
        /// <summary>
        /// Displays the date of the last treatment in the garage
        /// </summary>
        public DateTime LastCare { get => lastCare; set => lastCare = value; }
        /// <summary>
        /// Displays the amount of bus mileage in the last care
        /// </summary>
        public uint LastCareMileage { get => lastCareMileage; set => lastCareMileage = value; }
        /// <summary>
        /// A Ctor who creates a bus and also serves as a default Ctor
        /// </summary>
        /// <param name="dateRoadAscent"></param>
        /// <param name="id"></param>
        /// <param name="mileage"></param>
        /// <param name="fuel"></param>
        public Bus(DateTime dateRoadAscent = default, uint id = 0, uint mileage = 0, int fuel = 1200)
        {
            DateRoadAscent = dateRoadAscent;
            Id = id;
            Mileage = mileage;
            Fuel = fuel;
            LastCare = DateRoadAscent;
            LastCareMileage = mileage;
        }
        /// <summary>
        /// The function updates the last treatment date and saves its mileage
        /// </summary>
        public void Care()
        {
            LastCare = DateTime.Now;
            LastCareMileage = mileage;
            Console.WriteLine("You can drive now another 20,000 miles safely :)");

        }
        /// <summary>
        /// The function refuels the bus to a full tank
        /// </summary>
        public void Refueling()
        {
            Fuel = 1200;
            Console.WriteLine("You have now full tank :)");
        }
        /// <summary>
        /// The func. checks if the bus has passed a year since the last care 
        /// or 20 thousand kilometers
        /// </summary>
        ///<param name="addMileage"></param>
        /// <returns>If the bus need a care</returns>
        public bool CheckCare(uint addMileage)
        {
            if (Mileage + addMileage - LastCareMileage >= 20000)
                return false;
            else if (DateTime.Compare(DateTime.Now, lastCare.AddYears(1)) >= 0)
                return false;
            return true;
        }
        /// <summary>
        /// Shows whether a bus can travel with the fuel it has
        /// </summary>
        /// <param name="subFuel"></param>
        /// <returns>True or false accordingly</returns>
        public bool CheckFuel(uint subFuel)
        {
            if (Fuel - subFuel > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Adds the "addmileage" in the "mileage" and reduces the fuel accordingly
        /// </summary>
        /// <param name="addMileage"></param>
        public void StartDrive(uint addMileage)
        {
            Mileage += addMileage;
            Fuel -= (int)addMileage;
        }

        /// <summary>
        /// returns how meny mileage the bus was driving - from the last care
        /// </summary>
        /// <returns></returns>
        public uint ReturnLastCare()
        {
            return (Mileage - LastCareMileage);
        }
        /// <summary>
        ///A function converts the vehicle license to a string
        /// </summary>
        /// <returns>string at format 12-345-67(Until 2017 inclusive),123-45-678(2018 onwards)</returns>
        public string PrintId()
        {
            string temp = this.Id.ToString();
            if (this.DateRoadAscent.Year > 2017)
            {
                temp = temp.Insert(3, "-");
                temp = temp.Insert(6, "-");
            }
            if (this.DateRoadAscent.Year < 2018)
            {
                temp = temp.Insert(2, "-");
                temp = temp.Insert(6, "-");
            }
            return temp;
        }
    }
}
