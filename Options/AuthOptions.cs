using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Options
{
    public class ClientAuthCredential
    {
        public string ID { get; set; }
        public string Secret { get; set; }
    }

    public class AuthOptions
    {
        public static readonly string Name = "Auth";

        public string Authority { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int KeyRenewalDuration { get; set; }
        public string CertificatePath { get; set; }
        public string EncryptKey { get; set; }
        public ClientAuthCredential[] Clients { get; set; }
    }
}
