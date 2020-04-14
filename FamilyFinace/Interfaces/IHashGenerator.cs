using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IHashGenerator
    {
        string GetHash(string input);
    }
}
