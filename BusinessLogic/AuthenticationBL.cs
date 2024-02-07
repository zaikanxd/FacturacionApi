using BusinessEntity;
using DataAccess;

namespace BusinessLogic
{
    public class AuthenticationBL : InterfaceData.IAuthenticationDA
    {
        private AuthenticationDA _AuthenticationDA;
        private AuthenticationDA oAuthenticationDA
        {
            get { return (_AuthenticationDA == null ? _AuthenticationDA = new AuthenticationDA() : _AuthenticationDA); }
        }

        public AuthResponse login(AuthRequest pAuthRequest)
        {
            return oAuthenticationDA.login(pAuthRequest);
        }
    }
}
