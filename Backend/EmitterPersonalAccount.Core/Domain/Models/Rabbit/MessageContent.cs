﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public class MessageContent
    {
        public string? Subject { get; set; }
        public required string Text { get; set; }
    }
}
