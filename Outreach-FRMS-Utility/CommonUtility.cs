using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace Outreach_FRMS_Utility
{
    public class CommonUtility
    {
        /// <summary>
        /// Encrypt the password
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public string Encrypt(string clearText)
        {
            string EncryptionKey = Resources.EncryptionKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Decrypt the password
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = Resources.EncryptionKey;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        /// <summary>
        /// Save the image in database
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public string SaveImage(IList<IFormFile> files)
        {
            CommonUtility commonUtility = new CommonUtility();
            var response = "";
            foreach (var file in files)
            {
                if (files != null)
                {
                    //Getting FileNamed
                    var fileName = Path.GetFileName(file.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);

                    //var objfiles = new UserDocumentMapping()
                    //{
                    //    DocumentId = Convert.ToInt32(id),
                    //    DocumentName = fileName,
                    //    UserId = Convert.ToInt32(uid)
                    //};

                    using (var target = new MemoryStream())
                    {
                        byte[] p1 = null;
                        file.CopyTo(target);
                        p1 = target.ToArray();
                        response = Convert.ToBase64String(p1, Base64FormattingOptions.None);
                        // objfiles.DocumentImage = Convert.ToBase64String(p1, Base64FormattingOptions.None);
                        // response = _userService.UploadImage(objfiles);
                    }
                }
            }
            return response;
        }
        public string ImageToByte(string fileName)
        {
            FileStream fs1 = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs1);
            byte[] image = br.ReadBytes((int)fs1.Length);
            br.Close();
            fs1.Close();
            string result = Convert.ToBase64String(image, Base64FormattingOptions.None);
            return result;
        }
    }
}
