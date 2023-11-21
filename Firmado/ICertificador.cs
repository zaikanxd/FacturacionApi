using Dto.Intercambio;
using System.Threading.Tasks;

namespace Firmado
{
    public interface ICertificador
    {
        Task<FirmadoResponse> FirmarXml(FirmadoRequest request);
    }
}