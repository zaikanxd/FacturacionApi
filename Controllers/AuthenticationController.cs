using BusinessEntity;
using BusinessLogic;
using System.Web.Http;

namespace FacturacionApi.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("authentication")]
    public class AuthenticationController : ApiController
    {
        private AuthenticationBL _AuthenticationBL;
        private AuthenticationBL oAuthenticationBL
        {
            get { return (_AuthenticationBL == null ? _AuthenticationBL = new AuthenticationBL() : _AuthenticationBL); }
        }

        [HttpPost, Route("login")]
        public AuthResponse login([FromBody] AuthRequest pAuthRequest)
        {
            return oAuthenticationBL.login(pAuthRequest);
        }
    }
}