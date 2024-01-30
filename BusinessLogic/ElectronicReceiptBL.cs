using DataAccess;
using Dto.Intercambio;
using Dto.Modelos;
using InterfaceData;

namespace BusinessLogic
{
    public class ElectronicReceiptBL : IElectronicReceiptDA
    {
        private ElectronicReceiptDA _ElectronicReceiptDA;
        private ElectronicReceiptDA oElectronicReceiptDA
        {
            get { return (_ElectronicReceiptDA == null ? _ElectronicReceiptDA = new ElectronicReceiptDA() : _ElectronicReceiptDA); }
        }

        public void insertElectronicReceipt(EnviarDocumentoResponse pEnviarDocumentoResponse, DocumentoElectronico documento)
        {
            oElectronicReceiptDA.insertElectronicReceipt(pEnviarDocumentoResponse, documento);
        }
    }
}
