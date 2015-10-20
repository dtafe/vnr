using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infrastructure.Utilities.Helper
{
    public static class SharpZipLibExtensions
    {

        /// <summary>
        /// Add a File to the ZipFile
        /// </summary>
        /// <param name="z">ZipFile object</param>
        /// <param name="fileToZip">the fileinfo object to zip</param>
        public static void Add(this ZipFile z, FileInfo fileToZip)
        {
            Add(z, (FileSystemInfo)fileToZip);
        }

        /// <summary>
        /// Add a Directory to the ZipFile
        /// </summary>
        /// <param name="z">ZipFile object</param>
        /// <param name="directoryToZip">the DirectoryInfo object to zip</param>
        public static void Add(this ZipFile z, DirectoryInfo directoryToZip)
        {
            Add(z, (FileSystemInfo)directoryToZip);
        }

        /// <summary>
        /// Add a File/Directory to the ZipFile
        /// </summary>
        /// <param name="z">ZipFile object</param>
        /// <param name="fileSystemInfoToZip">the FileSystemInfo object to zip</param>
        public static void Add(this ZipFile z, FileSystemInfo fileSystemInfoToZip)
        {
            Add(z, new FileSystemInfo[] 
                            { 
                                fileSystemInfoToZip 
                            });
        }

        /// <summary>
        /// Add an array of File/Directory to the ZipFile
        /// </summary>
        /// <param name="z">ZipFile object</param>
        /// <param name="fileSystemInfoToZip">the FileSystemInfo object to zip</param>
        public static void Add(this ZipFile z, FileSystemInfo[] fileSystemInfosToZip)
        {
            GetFilesToZip(fileSystemInfosToZip, z);
        }

        /// <summary>
        /// Iterate thru all the filesysteminfo objects and add it to our zip file
        /// </summary>
        /// <param name="fileSystemInfosToZip">a collection of files/directores</param>
        /// <param name="z">our existing ZipFile object</param>
        private static void GetFilesToZip(FileSystemInfo[] fileSystemInfosToZip, ZipFile z)
        {
            //check whether the objects are null
            if (fileSystemInfosToZip != null && z != null)
            {
                //iterate thru all the filesystem info objects
                foreach (FileSystemInfo fi in fileSystemInfosToZip)
                {
                    //check if it is a directory
                    if (fi is DirectoryInfo)
                    {
                        DirectoryInfo di = (DirectoryInfo)fi;
                        //add the directory
                        z.AddDirectory(di.FullName);
                        //drill thru the directory to get all
                        //the files and folders inside it.
                        GetFilesToZip(di.GetFileSystemInfos(), z);
                    }
                    else
                    {
                        //add it
                        z.Add(fi.FullName);
                    }
                }
            }
        }
    }
}
