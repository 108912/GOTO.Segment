using System;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlTableItem")]
    public class SqlTableItemXml
    {
        [XmlElement("BaseNumber")]
        public long BaseNumber
        { get; set; }
        [XmlElement("ServerNumber")]
        public long ServerNumber
        { get; set; }
        [XmlElement("DataBaseNumber")]
        public long DataBaseNumber
        { get; set; }
        [XmlElement("Number")]
        public long Number
        { get; set; }
        [XmlElement("TableName")]
        public string TableName
        { get; set; }
        /// <summary>
        /// 存储空间(字节)
        /// </summary>
        [XmlElement("StorageSize")]
        public long StorageSize
        { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [XmlElement("IsUse")]
        public bool IsUse
        { get; set; }
        [XmlElement("IsWrite")]
        public bool IsWrite
        { get; set; }
        /// <summary>
        /// 数据表最大记录数
        /// </summary>
        [XmlElement("RowNum")]
        public long RowNum
        { get; set; }
    }
}