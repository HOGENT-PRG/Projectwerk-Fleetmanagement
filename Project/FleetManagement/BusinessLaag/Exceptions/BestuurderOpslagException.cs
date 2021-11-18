﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class BestuurderOpslagException : Exception {
        public BestuurderOpslagException(string msg) : base(msg) { }
        public BestuurderOpslagException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
