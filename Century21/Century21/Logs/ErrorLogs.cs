using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Century21.Logs
{
    public class ErrorLogs
    {
        public void WriteErrorLogs(string pathName, string errorMessage)
        {
            string Logpath = ConfigurationManager.AppSettings["LogPath"].ToString();
            string fileName = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            string filePath = fileName + Logpath;

            if (File.Exists(filePath))
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {

            }
        }
    }
}
