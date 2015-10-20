using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace HRM.Infrastructure.Utilities.Helper
{
    public static class CompressFile
    {
        public static void CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , FileInfo fileToCompress)
        {
            //create our zip file
            ZipFile z = ZipFile.Create(zipFileStoragePath + zipFileName);

            //initialize the file so that it can accept updates
            z.BeginUpdate();

            //add the file to the zip file
            z.Add(fileToCompress);

            //commit the update once we are done
            z.CommitUpdate();
            //close the file
            z.Close();
        }

        public static void CreateZipFile(string zipFileStoragePath
         , string zipFileName
         , string filepath, string password)
        {
            //create our zip file
            ZipFile z = ZipFile.Create(zipFileStoragePath + zipFileName);

            //initialize the file so that it can accept updates
            z.BeginUpdate();

            z.Password = password;
            //add the file to the zip file
            z.Add(filepath);

            //commit the update once we are done
            z.CommitUpdate();
            //close the file
            z.Close();
        }

        /// <summary>
        /// A method that creates a zip file
        /// </summary>
        /// <param name="zipFileStoragePath">the storage location</param>
        /// <param name="zipFileName">the zip file name</param>
        /// <param name="directoryToCompress">the directory to compress</param>
        public static void CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , DirectoryInfo directoryToCompress)
        {

            //copy thu muc
            string folderTemp = @"c:\" + Guid.NewGuid().ToString();
            if (!Directory.Exists(folderTemp))
                Directory.CreateDirectory(folderTemp);
            CopyDirectory(directoryToCompress.FullName, folderTemp, true);

            //Creat copy
            DirectoryInfo _directoryToCompress = new DirectoryInfo(folderTemp);
            //create our zip file
            ZipFile z = ZipFile.Create(zipFileStoragePath + @"\" + zipFileName);

            //initialize the file so that it can accept updates
            z.BeginUpdate();

            //add the directory to the zip file
            z.Add(_directoryToCompress);


            //commit the update once we are done
            z.CommitUpdate();
            //close the file
            z.Close();

            //Delete Forder temp
            try
            {
                if (Directory.Exists(folderTemp))
                    Directory.Delete(folderTemp, true);
            }
            catch (Exception)
            {
            }
        }
        public static void Zip(string SrcFile, string DstFile, int BufferSize)
        {
            FileStream fileStreamIn = new FileStream(SrcFile, FileMode.Open, FileAccess.Read);
            FileStream fileStreamOut = new FileStream(DstFile, FileMode.Create, FileAccess.Write);
            ZipOutputStream zipOutStream = new ZipOutputStream(fileStreamOut);

            byte[] buffer = new byte[BufferSize];

            ZipEntry entry = new ZipEntry(Path.GetFileName(SrcFile));
            zipOutStream.PutNextEntry(entry);

            int size;
            do
            {
                size = fileStreamIn.Read(buffer, 0, buffer.Length);
                zipOutStream.Write(buffer, 0, size);
            } while (size > 0);

            zipOutStream.Close();
            fileStreamOut.Close();
            fileStreamIn.Close();
        }
        public static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
    }
}
