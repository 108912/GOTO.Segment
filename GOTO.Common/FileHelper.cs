using System;
using System.Text;
using System.IO;

namespace GOTO.Common
{
    /// <summary>   
    /// 文件操作类   
    /// </summary>   
    public class FileHelper
    {
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        /// <summary>
        /// 检测指定文件是否存在
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }
        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符</param>
        /// <returns></returns>
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>   
        /// 创建一个目录   
        /// </summary>   
        /// <param name="directoryPath">目录的绝对路径</param>   
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        /// <summary>   
        /// 创建一个文件。   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static void CreateFile(string filePath)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Close();
                }
            }
            catch
            {
                ;
            }
        }
        /// <summary>   
        /// 创建一个文件,并将字节流写入文件  
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="buffer">二进制流数据</param>   
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }
            }
            catch
            {
                ;
            }
        }
        /// <summary>   
        /// 获取文本文件的行数   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static int GetLineCount(string filePath)
        {
            string[] rows = File.ReadAllLines(filePath);
            return rows.Length;
        }
        /// <summary>   
        /// 获取一个文件的长度,单位为Byte   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static int GetFileSize(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return (int)fi.Length;
        }
        /// <summary>   
        /// 获取一个文件的长度,单位为KB   
        /// </summary>   
        /// <param name="filePath">文件的路径</param>           
        public static double GetFileSizeByKB(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return CommonHelper.ToDouble(CommonHelper.ToDouble(fi.Length) / 1024, 1);
        }
        /// <summary>   
        /// 获取一个文件的长度,单位为MB   
        /// </summary>   
        /// <param name="filePath">文件的路径</param>           
        public static double GetFileSizeByMB(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return CommonHelper.ToDouble(CommonHelper.ToDouble(fi.Length) / 1024 / 1024, 1);
        }
        /// <summary>   
        /// 获取指定目录中所有文件列表   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>           
        public static string[] GetFileNames(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }
        /// <summary>   
        /// 获取指定目录及子目录中所有文件列表   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符     
        /// <param name="isSearchChild">是否搜索子目录</param>  
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件</param>  
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        /// <summary>   
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法 
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>           
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        /// <summary>   
        /// 获取指定目录及子目录中所有子目录列表   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符 
        /// <param name="isSearchChild">是否搜索子目录</param>     
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件</param>   
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        /// <summary>   
        /// 向文本文件中写入内容   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="content">写入的内容</param>           
        public static void WriteText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
        /// <summary>   
        /// 向文本文件的尾部追加内容   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="content">写入的内容</param>   
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        /// <summary>   
        /// 将源文件的内容复制到目标文件中   
        /// </summary>   
        /// <param name="sourceFilePath">源文件的绝对路径</param>   
        /// <param name="destFilePath">目标文件的绝对路径</param>   
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        /// <summary>   
        /// 将文件移动到指定目录   
        /// </summary>   
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>   
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>   
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            string sourceFileName = GetFileName(sourceFilePath);
            if (IsExistDirectory(descDirectoryPath))
            {
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        /// <summary>   
        /// 将流读取到缓冲区中   
        /// </summary>   
        /// <param name="stream">原始流</param>   
        public static byte[] StreamToBytes(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, CommonHelper.ToInt(stream.Length));
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        /// <summary>   
        /// 将文件读取到缓冲区中   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static byte[] FileToBytes(string filePath)
        {
            int fileSize = GetFileSize(filePath);
            byte[] buffer = new byte[fileSize];
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = fi.Open(FileMode.Open);
            try
            {
                fs.Read(buffer, 0, fileSize);
                return buffer;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                fs.Close();
            }
        }
        /// <summary>   
        /// 将文件读取到字符串中   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="encoding">字符编码</param>   
        public static string FileToString(string filePath, Encoding encoding)
        {
            StreamReader reader = new StreamReader(filePath, encoding);
            try
            {
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }
        /// <summary>   
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static string GetFileName(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        /// <summary>   
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static string GetFileNameNoExtension(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        /// <summary>   
        /// 从文件的绝对路径中获取扩展名   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static string GetExtension(string filePath)
        {
            //获取文件的名称   
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        /// <summary>   
        /// 清空指定目录下所有文件及子目录,但该目录依然保存 
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }
        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>  
        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            CreateFile(filePath);
        }
        /// <summary> 
        /// 删除指定文件   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>   
        /// 删除指定目录及其所有子目录
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }
}