using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Curse.RestProxy.Authentication;

namespace Curse.RestProxy.AddOnService
{
    public class TokenEndpointBehavior: IEndpointBehavior, IClientMessageInspector
    {
        private AuthenticationToken Token { get; set; }

        public TokenEndpointBehavior(TokenIdentity identity)
        {
            Token = new AuthenticationToken()
            {
                Token = identity.Token,
                UserID = identity.UserId
            };
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var header = MessageHeader.CreateHeader("AuthenticationToken", "urn:Curse.FriendsService:v1", Token);
            request.Headers.Add(header);
            return null;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        [DataContract(Namespace = "urn:Curse.FriendsService:v1")]
        private class AuthenticationToken
        {
            [DataMember]
            public int UserID { get; set; }

            [DataMember]
            public string Token { get; set; }

            [DataMember]
            public string ApiKey { get; set; }
        }
    }
}