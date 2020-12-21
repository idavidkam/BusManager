﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class DriverBO
    {
        /// <summary>
        /// Represents the unique number of the "Driver"
        /// </summary> 
        public int Id { get; set; }
        /// <summary>
        /// Represents the name of the driver
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Represents the seniority of the driver
        /// </summary>
        public int Seniority { get; set; }
    }
}
