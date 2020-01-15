using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RD.Lib
{
    public class RDFile
    {
        public static bool CreateFile(string p_sFilePath,
                                        string p_sContent,
                                        string p_sFileExtension = ".txt",
                                        bool p_bRewriteIfExist = false)
        {
            bool bIsSuccess = false;

            try
            {
                string sNewFilePath = p_sFilePath + p_sFileExtension;
                if (!File.Exists(sNewFilePath))
                {
                    FileInfo oFile = new System.IO.FileInfo(sNewFilePath);
                    oFile.Directory.Create();
                    System.IO.File.WriteAllText(oFile.FullName, p_sContent);
                }
                else
                {
                    if (p_bRewriteIfExist)
                    {
                        File.WriteAllText(sNewFilePath, p_sContent);
                    }
                }
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bIsSuccess;
        }

        public static void WriteToFile(
           string sPath,
           string sMessage,
           bool bIsNeedToSeparateLargeFile)
        {
            try
            {
                if (sPath != null &&
                    sPath.Trim().Length > 0)
                {
                    FileInfo oFile = new System.IO.FileInfo(sPath);

                    // New
                    if (!oFile.Exists)
                    {
                        oFile.Directory.Create();
                        System.IO.File.WriteAllText(oFile.FullName, sMessage);
                    }

                    // Existing
                    else
                    {
                        System.IO.FileInfo fiLogFile = new FileInfo(sPath);

                        if (bIsNeedToSeparateLargeFile
                            && fiLogFile.Length > 10485760)
                        {
                            // Rename
                            fiLogFile.CopyTo(string.Format("{0}_{1}.Bak",
                                sPath,
                                DateTime.Now.ToString("yyyyMMddHHmmss")), true);
                            fiLogFile.Delete();

                            // Create new
                            using (StreamWriter sw = System.IO.File.CreateText(sPath))
                            {
                                sw.WriteLine(sMessage);
                                sw.Close();
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = System.IO.File.AppendText(sPath))
                            {
                                sw.WriteLine(sMessage);
                                sw.Close();
                            }
                        }

                        fiLogFile = null;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        public static string ReadStringFromTxtFile(string p_strFile)
        {
            return File.ReadAllText(p_strFile);
        }

        public void EncryptFile(string p_sSourceFilePath, string p_sOutputFilePath, string p_sPassword)
        {
            byte[] bytesToBeEncrypted = File.ReadAllBytes(p_sSourceFilePath);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(p_sPassword);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = RDEncryption.AES_FileEncrypt(bytesToBeEncrypted, passwordBytes);

            string fileEncrypted = p_sOutputFilePath;

            File.WriteAllBytes(fileEncrypted, bytesEncrypted);
        }

        public void DecryptFile(string p_sSourceFilePath, string p_sOutputFilePath, string p_sPassword)
        {
            byte[] bytesToBeDecrypted = File.ReadAllBytes(p_sSourceFilePath);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(p_sPassword);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = RDEncryption.AES_FileDecrypt(bytesToBeDecrypted, passwordBytes);

            string file = p_sOutputFilePath;
            File.WriteAllBytes(file, bytesDecrypted);
        }
    }
}
