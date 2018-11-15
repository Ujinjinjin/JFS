﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Models.Requests.TFS
{
    public class TfsHook<T>
    {
        public string EventType { get; set; }
        public T Resource { get; set; }
    }
}
