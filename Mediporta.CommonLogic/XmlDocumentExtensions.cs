using Microsoft.Xades;
using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Mediporta.CommonLogic
{
    public static class XmlDocumentExtensions
    {
        public static XmlDocument Sign(this XmlDocument document, X509Certificate2 certificate)
        {
            var guid = Guid.NewGuid();
            var signedTime = DateTime.Now.ToLocalTime();

            var reference = new Reference
            {
                Uri = string.Empty,
                Id = "Reference_" + guid,
                Type = SignedXml.XmlDsigEnvelopedSignatureTransformUrl
            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());

            var signed = new XadesSignedXml(document);
            signed.AddReference(reference);
            signed.SignedInfo.Id = "SignedInfo_" + guid;
            signed.SigningKey = certificate.PrivateKey;

            var xadesObject = new XadesObject();
            xadesObject.QualifyingProperties.Target = "#" + "Signature_" + guid;
            xadesObject.QualifyingProperties.SignedProperties.Id = "SignedProperties_" + guid;
            FillXadesProperties(certificate, xadesObject.QualifyingProperties.SignedProperties.SignedSignatureProperties, signedTime);

            signed.AddXadesObject(xadesObject);

            signed.ComputeSignature();
            signed.Signature.Id = "Signature_" + guid;
            signed.KeyInfo = new KeyInfo();

            var keyData = new KeyInfoX509Data(certificate);
            keyData.AddSubjectName(certificate.SubjectName.Name);
            signed.KeyInfo.AddClause(keyData);

            var xml = signed.GetXml();
            var imported = document.ImportNode(xml, true);
            document.DocumentElement.AppendChild(imported);

            signed.XadesCheckSignature(XadesCheckSignatureMasks.CheckXmldsigSignature);
            signed.CheckSignature(certificate.PublicKey.Key);

            return document;
        }

        private static void FillXadesProperties(X509Certificate2 signingCertificate, SignedSignatureProperties signedSignatureProperties, DateTime signedTime)
        {
            var signingCert = CreateCertificate(signingCertificate);
            signedSignatureProperties.SigningCertificate.CertCollection.Add(signingCert);
            signedSignatureProperties.SigningTime = signedTime;
            signedSignatureProperties.SignaturePolicyIdentifier.SignaturePolicyImplied = true;
        }

        private static Cert CreateCertificate(X509Certificate2 signingCertificate)
        {
            var cert = new Cert();
            cert.IssuerSerial.X509IssuerName = signingCertificate.Issuer;
            var bigInt = new BigInteger(signingCertificate.GetSerialNumber());
            cert.IssuerSerial.X509SerialNumber = bigInt.ToString();
            cert.CertDigest.DigestMethod.Algorithm = SignedXml.XmlDsigSHA1Url;
            cert.CertDigest.DigestValue = signingCertificate.GetCertHash();

            return cert;
        }
    }
}
