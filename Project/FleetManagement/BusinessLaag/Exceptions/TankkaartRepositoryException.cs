﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class TankkaartRepositoryException : Exception
    {
        public TankkaartRepositoryException(string msg) : base(msg) { }
    }
}
