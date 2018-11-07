using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary
{
    public interface IListable  // this Interface is  create for the generic Dropdown list
    {
        int ID{ get; set; }
        string Name { get; set; }
    }
}
