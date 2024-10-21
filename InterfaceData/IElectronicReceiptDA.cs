using BusinessEntity;
using BusinessEntity.Dtos;
using Dto.Intercambio;
using Dto.Modelos;
using System;
using System.Collections.Generic;

namespace InterfaceData
{
    public interface IElectronicReceiptDA
    {
        void insertElectronicReceipt(EnviarDocumentoResponse pEnviarDocumentoResponse, DocumentoElectronico documento, string jsonPath);

        void updateElectronicReceipt(int id, EnviarDocumentoResponse pEnviarDocumentoResponse);

        List<ElectronicReceiptBE> getListPending();

        List<ElectronicReceiptBE> getListBy(DateTime date, string filter = null);

        ElectronicReceiptBE get(int id);

        void cancelElectronicReceipt(CancelElectronicReceiptRequest cancelElectronicReceiptRequest);

        string getJsonLink(JsonLinkRequest jsonLinkRequest);

        void updateCanceledCdrLink(UpdateCanceledCdrLinkRequest updateCanceledCdrLinkRequest);
    }
}
