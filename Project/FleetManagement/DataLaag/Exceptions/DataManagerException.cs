﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLaag.Exceptions
{
    public class DataManagerException : Exception
    {
        public DataManagerException(string msg): base(msg){}
    }
}
