using System;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlFieldItem")]
    public class SqlFieldItemXml
    {
        [XmlElement("BaseNumber")]
        public long BaseNumber
        { get; set; }
        [XmlElement("ServerNumber")]
        public long ServerNumber
        { get; set; }
        [XmlElement("DatabaseNumber")]
        public long DatabaseNumber
        { get; set; }
        [XmlElement("TableNumber")]
        public long TableNumber
        { get; set; }
        [XmlElement("FieldName")]
        public string FieldName
        { get; set; }
        [XmlElement("Number")]
        public long Number
        { get; set; }
        [XmlElement("ValueMin")]
        public long ValueMin
        { get; set; }
        [XmlElement("ValueMax")]
        public long ValueMax
        { get; set; }
        [XmlElement("IsDelete")]
        public bool IsDelete
        { get; set; }
    }
}