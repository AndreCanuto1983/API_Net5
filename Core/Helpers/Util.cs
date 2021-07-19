using System;

namespace Core.Helpers
{
    public static class Util
    {
        /// <summary>
        /// Convert uma string para byte[] e retorna um byte[]
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static byte[] ByteEncode(string text)
        {            
            var plainTextBytes = Base64EncodeToString(text);
            return Convert.FromBase64String(plainTextBytes);           
        }

        /// <summary>
        /// Desconverte byte[] para string e retorna a string
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string ByteDecode(byte[] base64EncodedData)
        {
            string ConvertByteToBase64 = Convert.ToBase64String(base64EncodedData);
            return Base64DecodeToString(ConvertByteToBase64);
        }

        /// <summary>
        /// Converte string para base64 e retorna como string
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private static string Base64EncodeToString(string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Desconverte string base64 para string e retorna como string
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        private static string Base64DecodeToString(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
