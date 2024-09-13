using BusinessEntity;
using BusinessEntity.Dtos;
using DataAccess;
using Dto.Intercambio;
using Dto.Modelos;
using InterfaceData;
using System;
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

        public void insertElectronicReceipt(EnviarDocumentoResponse pEnviarDocumentoResponse, DocumentoElectronico documento, string jsonPath)
        {
            oElectronicReceiptDA.insertElectronicReceipt(pEnviarDocumentoResponse, documento, jsonPath);
        }

        public void updateElectronicReceipt(int id, EnviarDocumentoResponse pEnviarDocumentoResponse)
        {
            oElectronicReceiptDA.updateElectronicReceipt(id, pEnviarDocumentoResponse);
        }

        public List<ElectronicReceiptBE> getListPending()
        {
            return oElectronicReceiptDA.getListPending();
        }

        public List<ElectronicReceiptBE> getListBy(DateTime date, string filter = null)
        {
            return oElectronicReceiptDA.getListBy(date, filter);
        }

        public ElectronicReceiptBE get(int id)
        {
            return oElectronicReceiptDA.get(id);
        }

        public void cancelElectronicReceipt(CancelElectronicReceiptRequest cancelElectronicReceiptRequest)
        {
            oElectronicReceiptDA.cancelElectronicReceipt(cancelElectronicReceiptRequest);
        }

        public string getJsonLink(JsonLinkRequest jsonLinkRequest)
        {
            return oElectronicReceiptDA.getJsonLink(jsonLinkRequest);
        }
    }
}
