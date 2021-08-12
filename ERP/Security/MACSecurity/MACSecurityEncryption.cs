using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Security.MACSecurity
{
    public class MACSecurityEncryption
    {
        string Mac = "";
        string appMAC = "";
        string Decryptcode = "";

        public bool ADMIN;
        public bool HR;
        public bool SALES;
        public bool INVENTORY;
        public bool MASTERS;
        public bool REPORTS;
        public bool PRODUCTION;




        //public bool 
        public MACSecurityEncryption()
        {
            macToString();
            Decryptcode = Decrypt(ConfigurationManager.AppSettings["ERP_Key"], true);
            DecryptcodeTokernizer();
        }

        void macToString()
        {
            try
            {
                //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                //PhysicalAddress address = GetMacAddress();
                //Mac = address.ToString();
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();
                String id = String.Empty;
                foreach (ManagementObject mo in moc)
                {

                    id = mo.Properties["processorID"].Value.ToString();
                    break;
                }
                Mac = id.ToString();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Cannot get the Mac Address");
                Mac = "";
            }
        }

        void DecryptcodeTokernizer()
        {
            string[] words = Decryptcode.Split(' ');
            appMAC = words.First();
            foreach (string word in words)
            {
                HomeSecurityCheck(word);
            }
        }

        void HomeSecurityCheck(String ActiveCode)
        {
            if (Mac == appMAC)
            {
                switch (ActiveCode)
                {
                    case "HR":
                        HR = true;
                        break;
                    case "PRODUCTION":
                        PRODUCTION = true;
                        break;
                    case "SALES":
                        SALES = true;
                        break;
                    case "INVENTORY":
                        INVENTORY = true;
                        break;
                    case "MASTERS":
                        MASTERS = true;
                        break;
                    case "REPORTS":
                        REPORTS = true;
                        break;
                    case "ADMIN":
                        ADMIN = true;
                        break;

                }
            }
        }

        public static PhysicalAddress GetMacAddress()
        {
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        return nic.GetPhysicalAddress();
                    //else if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    //    return nic.GetPhysicalAddress();

                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                System.Configuration.AppSettingsReader settingsReader =
                                                    new AppSettingsReader();
                //Get your key from config file to open the lock!
                string key = "H2SO4(:P)";

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Activation key is not available");
                return "";

            }
        }


    }
}
