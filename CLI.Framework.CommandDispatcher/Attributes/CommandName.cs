using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Framework.CommandDispatcher.Attributes
{
    public class CommandName : Attribute
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }

        public CommandName(string name)
        {
            Name = name;
        }
    }
}
