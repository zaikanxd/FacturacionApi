﻿using Dto.Intercambio;
using Dto.Modelos;

namespace InterfaceData
{
    public interface IElectronicReceiptDA
    {
        void insertElectronicReceipt(EnviarDocumentoResponse pEnviarDocumentoResponse, DocumentoElectronico documento);

        void updateElectronicReceipt(int id, EnviarDocumentoResponse pEnviarDocumentoResponse);
    }
}
