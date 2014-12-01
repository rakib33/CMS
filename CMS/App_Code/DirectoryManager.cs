using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;



namespace CMS.App_Code
{
    public class DirectoryManager
    {
      
       
        public void Directory_Manager(string DirName, string logFile,string message,string UserNo)  //logFile is full file name with dirname such as C://Log//a.txt
        {
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }

            String errTime = DateTime.Now.ToLongTimeString();
            //add log time
          //  variable.HeaderAll = "Time:" + errTime + variable.HeaderAll;

            var WriteTest = "=> Time" + errTime + " User No:" + UserNo+" message="+message;
            
            try
            {
                StreamWriter sw = new StreamWriter(logFile, true);
                sw.WriteLine(WriteTest);
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
            }
        
        }

    }
}