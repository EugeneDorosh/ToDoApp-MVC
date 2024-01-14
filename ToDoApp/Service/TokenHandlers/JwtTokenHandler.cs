using Service.Interface.ITokenHandler;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.TokenHandlers
{
    public class JwtTokenHandler : ITokenHandler
    {
        public bool TryGetIdFromJwtToken(string token, ref Guid id)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var handledToken = jwtHandler.ReadJwtToken(token);

            Claim claim = handledToken.Claims.FirstOrDefault(x => x.Type == "id");

            if (claim == null)
                return false;

            string idAsString = claim.Value;

            id = new Guid(idAsString);
            return true;
        }
    }
}
