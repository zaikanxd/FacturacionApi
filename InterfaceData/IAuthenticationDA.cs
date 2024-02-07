using BusinessEntity;

namespace InterfaceData
{
    public interface IAuthenticationDA
    {
        AuthResponse login(AuthRequest pAuthRequest);
    }
}