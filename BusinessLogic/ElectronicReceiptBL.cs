using BusinessEntity;
using DataAccess;
using Dto.Intercambio;
using Dto.Modelos;
using InterfaceData;
using System.Collections.Generic;

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

        public void updateElectronicReceipt(int id, EnviarDocumentoResponse pEnviarDocumentoResponse)
        {
            oElectronicReceiptDA.updateElectronicReceipt(id, pEnviarDocumentoResponse);
        }

        public List<ElectronicReceiptBE> getListPending()
        {
            return oElectronicReceiptDA.getListPending();
        }
    }
}
