using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceReference
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://service.sunat.gob.pe", ConfigurationName = "Documentos.billService")]
    public interface billService
    {

        // CODEGEN: Parameter 'status' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "urn:getStatus", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "status")]
        ServiceReference.getStatusResponse getStatus(ServiceReference.getStatusRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "urn:getStatus", ReplyAction = "*")]
        System.Threading.Tasks.Task<ServiceReference.getStatusResponse> getStatusAsync(ServiceReference.getStatusRequest request);

        // CODEGEN: Parameter 'applicationResponse' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "urn:sendBill", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "applicationResponse")]
        ServiceReference.sendBillResponse sendBill(ServiceReference.sendBillRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "urn:sendBill", ReplyAction = "*")]
        System.Threading.Tasks.Task<ServiceReference.sendBillResponse> sendBillAsync(ServiceReference.sendBillRequest request);

        // CODEGEN: Parameter 'ticket' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "urn:sendPack", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "ticket")]
        ServiceReference.sendPackResponse sendPack(ServiceReference.sendPackRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "urn:sendPack", ReplyAction = "*")]
        System.Threading.Tasks.Task<ServiceReference.sendPackResponse> sendPackAsync(ServiceReference.sendPackRequest request);

        // CODEGEN: Parameter 'ticket' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "urn:sendSummary", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "ticket")]
        ServiceReference.sendSummaryResponse sendSummary(ServiceReference.sendSummaryRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "urn:sendSummary", ReplyAction = "*")]
        System.Threading.Tasks.Task<ServiceReference.sendSummaryResponse> sendSummaryAsync(ServiceReference.sendSummaryRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.sunat.gob.pe")]
    public partial class statusResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private byte[] contentField;

        private string statusCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 0)]
        public byte[] content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
                this.RaisePropertyChanged("statusCode");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getStatus", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class getStatusRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ticket;

        public getStatusRequest()
        {
        }

        public getStatusRequest(string ticket)
        {
            this.ticket = ticket;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getStatusResponse", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class getStatusResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ServiceReference.statusResponse status;

        public getStatusResponse()
        {
        }

        public getStatusResponse(ServiceReference.statusResponse status)
        {
            this.status = status;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "sendBill", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class sendBillRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileName;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] contentFile;

        public sendBillRequest()
        {
        }

        public sendBillRequest(string fileName, byte[] contentFile)
        {
            this.fileName = fileName;
            this.contentFile = contentFile;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "sendBillResponse", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class sendBillResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] applicationResponse;

        public sendBillResponse()
        {
        }

        public sendBillResponse(byte[] applicationResponse)
        {
            this.applicationResponse = applicationResponse;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "sendPack", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class sendPackRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileName;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] contentFile;

        public sendPackRequest()
        {
        }

        public sendPackRequest(string fileName, byte[] contentFile)
        {
            this.fileName = fileName;
            this.contentFile = contentFile;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "sendPackResponse", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class sendPackResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ticket;

        public sendPackResponse()
        {
        }

        public sendPackResponse(string ticket)
        {
            this.ticket = ticket;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "sendSummary", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class sendSummaryRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileName;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] contentFile;

        public sendSummaryRequest()
        {
        }

        public sendSummaryRequest(string fileName, byte[] contentFile)
        {
            this.fileName = fileName;
            this.contentFile = contentFile;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "sendSummaryResponse", WrapperNamespace = "http://service.sunat.gob.pe", IsWrapped = true)]
    public partial class sendSummaryResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.sunat.gob.pe", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ticket;

        public sendSummaryResponse()
        {
        }

        public sendSummaryResponse(string ticket)
        {
            this.ticket = ticket;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface billServiceChannel : ServiceReference.billService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class billServiceClient : System.ServiceModel.ClientBase<ServiceReference.billService>, ServiceReference.billService
    {

        public billServiceClient()
        {
        }

        public billServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public billServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public billServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public billServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference.getStatusResponse ServiceReference.billService.getStatus(ServiceReference.getStatusRequest request)
        {
            return base.Channel.getStatus(request);
        }

        public ServiceReference.statusResponse getStatus(string ticket)
        {
            ServiceReference.getStatusRequest inValue = new ServiceReference.getStatusRequest();
            inValue.ticket = ticket;
            ServiceReference.getStatusResponse retVal = ((ServiceReference.billService)(this)).getStatus(inValue);
            return retVal.status;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getStatusResponse> ServiceReference.billService.getStatusAsync(ServiceReference.getStatusRequest request)
        {
            return base.Channel.getStatusAsync(request);
        }

        public System.Threading.Tasks.Task<ServiceReference.getStatusResponse> getStatusAsync(string ticket)
        {
            ServiceReference.getStatusRequest inValue = new ServiceReference.getStatusRequest();
            inValue.ticket = ticket;
            return ((ServiceReference.billService)(this)).getStatusAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference.sendBillResponse ServiceReference.billService.sendBill(ServiceReference.sendBillRequest request)
        {
            return base.Channel.sendBill(request);
        }

        public byte[] sendBill(string fileName, byte[] contentFile)
        {
            ServiceReference.sendBillRequest inValue = new ServiceReference.sendBillRequest();
            inValue.fileName = fileName;
            inValue.contentFile = contentFile;
            ServiceReference.sendBillResponse retVal = ((ServiceReference.billService)(this)).sendBill(inValue);
            return retVal.applicationResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.sendBillResponse> ServiceReference.billService.sendBillAsync(ServiceReference.sendBillRequest request)
        {
            return base.Channel.sendBillAsync(request);
        }

        public System.Threading.Tasks.Task<ServiceReference.sendBillResponse> sendBillAsync(string fileName, byte[] contentFile)
        {
            ServiceReference.sendBillRequest inValue = new ServiceReference.sendBillRequest();
            inValue.fileName = fileName;
            inValue.contentFile = contentFile;
            return ((ServiceReference.billService)(this)).sendBillAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference.sendPackResponse ServiceReference.billService.sendPack(ServiceReference.sendPackRequest request)
        {
            return base.Channel.sendPack(request);
        }

        public string sendPack(string fileName, byte[] contentFile)
        {
            ServiceReference.sendPackRequest inValue = new ServiceReference.sendPackRequest();
            inValue.fileName = fileName;
            inValue.contentFile = contentFile;
            ServiceReference.sendPackResponse retVal = ((ServiceReference.billService)(this)).sendPack(inValue);
            return retVal.ticket;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.sendPackResponse> ServiceReference.billService.sendPackAsync(ServiceReference.sendPackRequest request)
        {
            return base.Channel.sendPackAsync(request);
        }

        public System.Threading.Tasks.Task<ServiceReference.sendPackResponse> sendPackAsync(string fileName, byte[] contentFile)
        {
            ServiceReference.sendPackRequest inValue = new ServiceReference.sendPackRequest();
            inValue.fileName = fileName;
            inValue.contentFile = contentFile;
            return ((ServiceReference.billService)(this)).sendPackAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference.sendSummaryResponse ServiceReference.billService.sendSummary(ServiceReference.sendSummaryRequest request)
        {
            return base.Channel.sendSummary(request);
        }

        public string sendSummary(string fileName, byte[] contentFile)
        {
            ServiceReference.sendSummaryRequest inValue = new ServiceReference.sendSummaryRequest();
            inValue.fileName = fileName;
            inValue.contentFile = contentFile;
            ServiceReference.sendSummaryResponse retVal = ((ServiceReference.billService)(this)).sendSummary(inValue);
            return retVal.ticket;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.sendSummaryResponse> ServiceReference.billService.sendSummaryAsync(ServiceReference.sendSummaryRequest request)
        {
            return base.Channel.sendSummaryAsync(request);
        }

        public System.Threading.Tasks.Task<ServiceReference.sendSummaryResponse> sendSummaryAsync(string fileName, byte[] contentFile)
        {
            ServiceReference.sendSummaryRequest inValue = new ServiceReference.sendSummaryRequest();
            inValue.fileName = fileName;
            inValue.contentFile = contentFile;
            return ((ServiceReference.billService)(this)).sendSummaryAsync(inValue);
        }
    }
}