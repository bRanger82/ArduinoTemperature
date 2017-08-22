using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature_Retrofit
{
    /// <summary>
    /// Contains functionality to log data into a file
    /// </summary>
    static public class LOG
    {
        public enum LogDataReturnValue
        {
            OK = 0,
            NO_WRITE_PERMISSION = 1,
            LOG_NOT_ENABLED = 2
        }

        static public LogDataReturnValue LogData(DataObject dobj, string data)
        {
            if (!dobj.LoggingEnabled)
                return LogDataReturnValue.LOG_NOT_ENABLED;

            string path = dobj.LogPath;
            FileInfo fi = new FileInfo(path);

            if (!Permission.HasAccess(fi, FileSystemRights.WriteData))
            {
                //Has no access to directory
                return LogDataReturnValue.NO_WRITE_PERMISSION;
            }

            //if directory not exists, create it
            if (!Directory.Exists(path.Substring(0, path.LastIndexOf("\\"))))
                Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));

            //if file not exists, create an empty one (streamwriter object needs an existing file for appending)
            if (!File.Exists(path))
                File.Create(path).Close();

            //if max filesize is reached, delete the logfile
            if (fi.Length > dobj.maxLogFileSize)
                fi.Delete();

            //append data-string to log
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(data);
            }

            return LogDataReturnValue.OK;

        }
    }

}
