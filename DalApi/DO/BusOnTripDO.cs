﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DO
{
    /// <summary>
    /// The class presents a bus on the trip, in the DO layer
    /// </summary>
    public class BusOnTrip
    {
        /// <summary>
        ///  Represents if this class active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Represents the unique number of the "BusOnTrip"
        /// </summary>
        public int IdTrip { get; set; }
        /// <summary>
        /// Represents the unique number of the "Bus"
        /// </summary>
        public int LinceseNum { get; set; }
        /// <summary>
        /// Represents the unique number of the "Line"
        /// </summary>
        public int IdLine { get; set; }
        /// <summary>
        /// Represents the time of the start drive  
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// Represents the time of the actual start drive  
        /// </summary>
        public TimeSpan RealSatrtTime { get; set; }
        /// <summary>
        /// Represents the unique number of the prev stop/station
        /// </summary>
        public int PrevNumStop { get; set; }
        /// <summary>
        /// Represents the time when the bus passed in prev stop/station 
        /// </summary>
        public TimeSpan PrevTimeStop { get; set; }
        /// <summary>
        /// Represents the time when the bus will arrive in next stop/station 
        /// </summary>
        public TimeSpan NextTimeStop { get; set; }
        /// <summary>
        /// Represents the unique number of the "Driver"
        /// </summary>
        public int IdDriver { get; set; }
    }
}
