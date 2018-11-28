using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Encrypt
{
    public class Crypt
    {

    #region Criptografia de string

        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Gera um hash MD5 de um array de 128 bits

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Configura o codificador
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Converte a entrada de string em byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Criptografa a string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Limpa os seviços de criptografia 
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Retorna uma string criptografada na codificação de base64
            return Convert.ToBase64String(Results);
        }

    #endregion

    #region Decriptografia de string

        public static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Gera um hash MD5 de um array de 128 bits

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Cria um objeto de TripleDESCryptoServiceProvider
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Configuração do decript
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Converte a string de entrada em byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Decriptografa a string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Limpa os serviços de TripleDes and Hashprovider
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Retorna a string decriptografada no format UTF8
            return UTF8.GetString(Results);
        }

        #endregion

        static void Main(string[] args)
        {
        }
   
    }
}

