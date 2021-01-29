using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace HyosungManagement.Services
{
    public class RSAKeyService
    {
        /// <summary>
        /// This points to a JSON file in the format: 
        /// {
        ///  "Modulus": "",
        ///  "Exponent": "",
        ///  "P": "",
        ///  "Q": "",
        ///  "DP": "",
        ///  "DQ": "",
        ///  "InverseQ": "",
        ///  "D": ""
        /// }
        /// </summary>
        private string KeyFile
        {
            get
            {
                return Path.Combine(environment.ContentRootPath, "RSAKey.json");
            }
        }
        private readonly IWebHostEnvironment environment;
        private readonly TimeSpan timeSpan;

        public RSAKeyService(IWebHostEnvironment environment, TimeSpan timeSpan)
        {
            this.environment = environment;
            this.timeSpan = timeSpan;
        }

        public bool NeedsUpdate()
        {
            if (File.Exists(KeyFile))
            {
                var creationDate = File.GetCreationTime(KeyFile);
                return DateTime.Now.Subtract(creationDate) > timeSpan;
            }
            return true;
        }

        public RSAParameters GetRandomKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    return rsa.ExportParameters(true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public RSAKeyService GenerateKeyAndSave(bool forceUpdate = false)
        {
            if (forceUpdate || NeedsUpdate())
            {
                var p = GetRandomKey();
                RSAParametersWithPrivate t = new RSAParametersWithPrivate();
                t.SetParameters(p);
                File.WriteAllText(KeyFile, JsonConvert.SerializeObject(t, Formatting.Indented));
            }
            return this;
        }

        /// <summary>
        /// 
        /// Generate 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public RSAParameters GetKeyParameters()
        {
            if (!File.Exists(KeyFile))
                throw new FileNotFoundException("Check configuration - cannot find auth key file: " + KeyFile);
            var keyParams = JsonConvert.DeserializeObject<RSAParametersWithPrivate>(File.ReadAllText(KeyFile));
            return keyParams.ToRSAParameters();
        }

        public RsaSecurityKey GetKey()
        {
            if (NeedsUpdate())
                GenerateKeyAndSave();

            var provider = new RSACryptoServiceProvider();
            provider.ImportParameters(GetKeyParameters());

            return new RsaSecurityKey(provider);
        }


        /// <summary>
        /// Util class to allow restoring RSA parameters from JSON as the normal
        /// RSA parameters class won't restore private key info.
        /// </summary>
        private class RSAParametersWithPrivate
        {
            public byte[] D { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }

            public void SetParameters(RSAParameters p)
            {
                D = p.D;
                DP = p.DP;
                DQ = p.DQ;
                Exponent = p.Exponent;
                InverseQ = p.InverseQ;
                Modulus = p.Modulus;
                P = p.P;
                Q = p.Q;
            }
            public RSAParameters ToRSAParameters()
            {
                return new RSAParameters()
                {
                    D = this.D,
                    DP = this.DP,
                    DQ = this.DQ,
                    Exponent = this.Exponent,
                    InverseQ = this.InverseQ,
                    Modulus = this.Modulus,
                    P = this.P,
                    Q = this.Q

                };
            }
        }
    }
}