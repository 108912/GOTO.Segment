using System;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlDatabaseItem")]
    public class SqlDatabaseItemXml
    {
        [XmlElement("BaseNumber")]
        public long BaseNumber
        { get; set; }
        [XmlElement("ServerNumber")]
        public long ServerNumber
        { get; set; }
        [XmlElement("Number")]
        public long Number
        { get; set; }
        [XmlElement("DatabaseName")]
        public string DatabaseName
        { get; set; }
        /// <summary>
        /// 存储空间(字节)
        /// </summary>
        [XmlElement("StorageSize")]
        public long StorageSize
        { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        [XmlElement("StorageDirectory")]
        public string StorageDirectory
        { get; set; }
        [XmlElement("IsWrite")]
        public bool IsWrite
        { get; set; }
        /// <summary>
        /// 数据表最大个数
        /// </summary>
        [XmlElement("TableNum")]
        public long TableNum
        { get; set; }
        /// <summary>
        /// 数据库文件目录
        /// </summary>
        [XmlElement("FilePath")]
        public string FilePath
        { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [XmlElement("IsUse")]
        public bool IsUse
        { get; set; }
        /// <summary>
        /// 是否自动创建表
        /// </summary>
        [XmlElement("IsCreate")]
        public bool IsCreate
        { get; set; }
    }
}