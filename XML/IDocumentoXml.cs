using Comun;
using Dto.Contratos;

namespace XML
{
    public interface IDocumentoXml
    {
        IEstructuraXml Generar(IDocumentoElectronico request);
    }
}