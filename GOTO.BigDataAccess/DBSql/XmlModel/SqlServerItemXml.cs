using System;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlServerItem")]
    public class SqlServerItemXml
    {
        [XmlElement("BaseNumber")]
        public long BaseNumber 
        { get; set; }
        [XmlElement("Number")]
        public long Number
        { get; set; }
        [XmlElement("ServerName")]
        public string ServerName
        { get; set; }
        [XmlElement("ServerPort")]
        public int ServerPort
        { get; set; }
        [XmlElement("ServerUserName")]
        public string ServerUserName
        { get; set; }
        [XmlElement("ServerUserPwd")]
        public string ServerUserPwd
        { get; set; }
        /// <summary>
        /// 存储空间(字节)
        /// </summary>
        [XmlElement("StorageSize")]
        public long StorageSize
        { get; set; }
        /// <summary>
        /// 保留剩余空间(字节)
        /// </summary>
        [XmlElement("RetainStorageSize")]
        public long RetainStorageSize
        { get; set; }
        /// <summary>
        /// 数据库最大个数
        /// </summary>
        [XmlElement("DatabaseNum")]
        public long DatabaseNum
        { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        [XmlElement("DBType")]
        public string DBType
        { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        [XmlElement("StorageDirectory")]
        public string StorageDirectory
        { get; set; }
        /// <summary>
        /// 是否可写入
        /// </summary>
        [XmlElement("IsWrite")]
        public bool IsWrite
        { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [XmlElement("IsUse")]
        public bool IsUse
        { get; set; }
        /// <summary>
        /// 是否自动创建库
        /// </summary>
        [XmlElement("IsCreate")]
        public bool IsCreate
        { get; set; }
    }
}