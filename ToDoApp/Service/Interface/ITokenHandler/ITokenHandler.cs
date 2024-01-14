using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface.ITokenHandler
{
    public interface ITokenHandler
    {
        public bool TryGetIdFromJwtToken(string token, ref Guid id);
    }
}
