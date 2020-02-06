using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

using Win32;

namespace FileHelper
{
    public class Win32FileHelper
    {
        public class Win32FileHandleHelper : IDisposable
        {
            public IntPtr Handle { get; set; } = IntPtr.Zero;
            public Win32FileHandleHelper(string dirpath)
            {
                Handle = OpenFileHandle(dirpath);
            }
            ~Win32FileHandleHelper()
            {
                this.Dispose();
            }
            public void Dispose()
            {
                if (Handle != IntPtr.Zero)
                {
                    CloseFileHandle(Handle);
                    Handle = IntPtr.Zero;
                }
            }

        }
        public class Win32FileTimeInfo
        {
            public Win32FileTimeInfo(string filepath)
            {
                try
                {
                    using (Win32FileHandleHelper hDir = new Win32FileHandleHelper(filepath))
                    {
                        FILETIME lpCreationTime = new FILETIME();
                        FILETIME lpLastAccessTime = new FILETIME();
                        FILETIME lpLastWriteTime = new FILETIME();
                        Kernel32.GetFileTime(hDir.Handle, ref lpCreationTime, ref lpLastAccessTime, ref lpLastWriteTime);
                        CreationTime = FileTimeToDateTime(lpCreationTime);
                        LastAccessTime = FileTimeToDateTime(lpLastAccessTime);
                        LastWriteTime = FileTimeToDateTime(lpLastWriteTime);
                    }
                }
                catch
                {
                }
            }
            public DateTime CreationTime { get; private set; } = DateTime.MaxValue;
            public DateTime LastAccessTime { get; private set; } = DateTime.MaxValue;
            public DateTime LastWriteTime { get; private set; } = DateTime.MaxValue;
        }

        static public DateTime FileTimeToDateTime(FILETIME FileTime)
        {
            long FileTimeLong = (((long)FileTime.dwHighDateTime) << 32) | ((long)FileTime.dwLowDateTime);
            if (FileTimeLong == 0)
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.FromFileTime(FileTimeLong);
            }
        }
        static public DateTime GetFileCreationTime(string filepath)
        {
            try
            {
                using (Win32FileHandleHelper hDir = new Win32FileHandleHelper(filepath))
                {
                    FILETIME lpCreationTime = new FILETIME();
                    FILETIME lpLastAccessTime = new FILETIME();
                    FILETIME lpLastWriteTime = new FILETIME();
                    Kernel32.GetFileTime(hDir.Handle, ref lpCreationTime, ref lpLastAccessTime, ref lpLastWriteTime);
                    return FileTimeToDateTime(lpCreationTime);
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        static public DateTime GetFileLastAccessTime(string filepath)
        {
            try
            {
                using (Win32FileHandleHelper hDir = new Win32FileHandleHelper(filepath))
                {
                    FILETIME lpCreationTime = new FILETIME();
                    FILETIME lpLastAccessTime = new FILETIME();
                    FILETIME lpLastWriteTime = new FILETIME();
                    Kernel32.GetFileTime(hDir.Handle, ref lpCreationTime, ref lpLastAccessTime, ref lpLastWriteTime);
                    return FileTimeToDateTime(lpLastAccessTime);
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        static public DateTime GetFileLastWriteTime(string filepath)
        {
            try
            {
                using (Win32FileHandleHelper hDir = new Win32FileHandleHelper(filepath))
                {
                    FILETIME lpCreationTime = new FILETIME();
                    FILETIME lpLastAccessTime = new FILETIME();
                    FILETIME lpLastWriteTime = new FILETIME();
                    Kernel32.GetFileTime(hDir.Handle, ref lpCreationTime, ref lpLastAccessTime, ref lpLastWriteTime);
                    return FileTimeToDateTime(lpLastWriteTime);
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        static public IntPtr OpenFileHandle(string filepath)
        {
            SECURITY_ATTRIBUTES Security = new SECURITY_ATTRIBUTES();
            int Attributes = Kernel32.GetFileAttributes(filepath);
            if ((Attributes & Kernel32.FILE_ATTRIBUTE_DIRECTORY) == Kernel32.FILE_ATTRIBUTE_DIRECTORY)
            {
                IntPtr hFile = (IntPtr)Kernel32.CreateFile(GetSafeLongPath(filepath),
                    Kernel32.GENERIC_READ,
                    Kernel32.FILE_SHARE_READ,
                    ref Security,
                    Kernel32.OPEN_EXISTING,
                    Kernel32.FILE_FLAG_BACKUP_SEMANTICS,
                    IntPtr.Zero);
                return hFile;
            }
            else
            {
                IntPtr hFile = (IntPtr)Kernel32.CreateFile(GetSafeLongPath(filepath),
                    Kernel32.GENERIC_READ,
                    Kernel32.FILE_SHARE_READ,
                    ref Security,
                    Kernel32.OPEN_EXISTING,
                    Attributes,
                    IntPtr.Zero);
                return hFile;
            }
        }
        static public void CloseFileHandle(IntPtr Handle)
        {
            Kernel32.CloseHandle(Handle);
        }
        static public bool IsDirectory(string filepath)
        {
            int Attributes = Kernel32.GetFileAttributes(filepath);
            return ((Attributes & Kernel32.FILE_ATTRIBUTE_DIRECTORY) == Kernel32.FILE_ATTRIBUTE_DIRECTORY);
        }

        static public string[] GetDirectories(string directoryPath, string pattern = @"*")
        {
            return GetDirectories(directoryPath, pattern, SearchOption.TopDirectoryOnly);
        }

        static public string[] GetDirectories(string directoryPath, SearchOption searchOption)
        {
            return GetDirectories(directoryPath, @"*", searchOption);
        }
        static public string[] GetDirectories(string directoryPath, string pattern, SearchOption searchOption)
        {
            string safeDirectoryPath = GetSafeLongPath(directoryPath);

            var results = new List<string>();
            IntPtr findHandle = (IntPtr)Kernel32.FindFirstFileEx(safeDirectoryPath.TrimEnd('\\') + @"\" + pattern, FINDEX_INFO_LEVELS.FindExInfoBasic, out WIN32_FIND_DATA lpFindFileData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, Kernel32.FIND_FIRST_EX_LARGE_FETCH);

            if (findHandle != (IntPtr)Kernel32.INVALID_HANDLE_VALUE)
            {
                try
                {
                    bool found;
                    do
                    {
                        var currentFileName = lpFindFileData.cFileName;

                        if (((int)lpFindFileData.dwFileAttributes & Kernel32.FILE_ATTRIBUTE_DIRECTORY) != 0)
                        {
                            if (currentFileName != @"." && currentFileName != @"..")
                            {
                                results.Add(Path.Combine(safeDirectoryPath, currentFileName));
                            }
                        }
                        found = Kernel32.FindNextFile(findHandle, out lpFindFileData) != 0;
                    } while (found);
                }
                finally
                {
                    Kernel32.FindClose(findHandle);
                }
            }


            if (searchOption == SearchOption.AllDirectories)
            {
                foreach (var dir in GetDirectories(safeDirectoryPath))
                {
                    results.AddRange(GetDirectories(dir, pattern, searchOption));
                }
            }
            return results.ToArray();
        }

        static public string[] GetFiles(string directoryPath, string pattern = @"*.*")
        {
            return GetFiles(directoryPath, pattern, SearchOption.TopDirectoryOnly);
        }

        static public string[] GetFiles(string directoryPath, SearchOption searchOption)
        {
            return GetFiles(directoryPath, @"*.*", searchOption);
        }
        public static string[] GetFiles(string directoryPath, string pattern, SearchOption searchOption)
        {
            string safeDirectoryPath = GetSafeLongPath(directoryPath);

            var results = new List<string>();
            
            IntPtr findHandle = (IntPtr)Kernel32.FindFirstFileEx(safeDirectoryPath.TrimEnd('\\') + @"\" + pattern, FINDEX_INFO_LEVELS.FindExInfoBasic, out WIN32_FIND_DATA lpFindFileData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, Kernel32.FIND_FIRST_EX_LARGE_FETCH);

            if (findHandle != (IntPtr)Kernel32.INVALID_HANDLE_VALUE)
            {
                try
                {
                    bool found;
                    do
                    {
                        var currentFileName = lpFindFileData.cFileName;

                        // if this is a file, find its contents
                        if (((int)lpFindFileData.dwFileAttributes & Kernel32.FILE_ATTRIBUTE_DIRECTORY) == 0)
                        {
                            results.Add(Path.Combine(safeDirectoryPath, currentFileName));
                        }

                        // find next
                        found = Kernel32.FindNextFile(findHandle, out lpFindFileData) != 0;
                    } while (found);
                }
                finally
                {
                    // close the find handle
                    Kernel32.FindClose(findHandle);
                }
            }

            if (searchOption == SearchOption.AllDirectories)
            {
                foreach (var dir in GetDirectories(safeDirectoryPath))
                {
                    results.AddRange(GetFiles(dir, pattern, searchOption));
                }
            }

            return results.ToArray();
        }

        static private string GetSafeLongPath(string path)
        {
            if (string.IsNullOrEmpty(path) || path.StartsWith(@"\\?\"))
            {
                return path;
            }
            else if (path.Length > Kernel32.MAX_PATH || path.Contains(@"~"))
            {
                if (path.StartsWith(@"\\"))
                {
                    return @"\\?\UNC\" + path.Substring(2);
                }
                else
                {
                    return @"\\?\" + path;
                }
            }
            else
            {
                return path;
            }
        }


    }
}
