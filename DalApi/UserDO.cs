﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
   
    /// <summary>
    /// The class represents a user in program, in the DO layer
    /// </summary>
    public class User
    {
        /// <summary>
        ///  Represents if this class active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Represents the unique user name of the "User"
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Represents the unique password of the "User"
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Represents the authorization of the "User"
        /// </summary>
        public Authorizations Authorization { get; set; }
    }
}