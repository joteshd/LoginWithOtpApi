using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace Services.ServiceUtilities
{
    internal static class Mapper 
    {
        public static Destinbation Clone<Source, Destinbation>(Source source, Destinbation destinbation) where Source : class
        where Destinbation : class
        {

            var sourceProperties = source.GetType().GetProperties();
            var destProperties = destinbation.GetType().GetProperties();
            
            foreach ( var property in sourceProperties )
            {
                destProperties.Where(w => w.Name.Equals(property.Name)).
                    FirstOrDefault()?.
                    SetValue(destinbation,property.GetValue(source, null));
            }

            return destinbation;
        }
    }
}
