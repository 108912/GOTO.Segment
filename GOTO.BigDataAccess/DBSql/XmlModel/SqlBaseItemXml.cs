using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlBaseItem")]
    public class SqlBaseItemXml
    {
        [XmlElement("Number")]
        public long Number
        { get; set; }
        [XmlElement("SocketMillisecondsTimeout")]
        public int SocketMillisecondsTimeout
        { get; set; }
        [XmlElement("DatabasePrev")]
        public string DatabasePrev
        { get; set; }
        [XmlElement("TablePrev")]
        public string TablePrev
        { get; set; }
        [XmlElement("IndexPrev")]
        public string IndexPrev
        { get; set; }

        [XmlElement("PrimarykeyName")]
        public string PrimarykeyName
        { get; set; }
        [XmlElement("FieldIndexList")]
        public List<string> FieldIndexList
        { get; set; }
        [XmlElement("FieldUpdateList")]
        public List<string> FieldUpdateList
        { get; set; }
        [XmlElement("FileDirDefault")]
        public string FileDirDefault
        { get; set; }
        [XmlElement("DefaultTableNum")]
        public long DefaultTableNum
        { get; set; }
        [XmlElement("DefaultTableRowNum")]
        public long DefaultTableRowNum
        { get; set; }
        [XmlElement("TemplateSqlConnServer")]
        public string TemplateSqlConnServer
        { get; set; }
        [XmlElement("TemplateSqlConnDatabase")]
        public string TemplateSqlConnDatabase
        { get; set; }
        [XmlElement("ServerFilePath")]
        public string ServerFilePath
        { get; set; }
        [XmlElement("DatabaseFilePath")]
        public string DatabaseFilePath
        { get; set; }
        [XmlElement("TableFilePath")]
        public string TableFilePath
        { get; set; }
        [XmlElement("FieldFilePath")]
        public string TableSqlFilePath
        { get; set; }

        [XmlElement("TableSqlFilePath")]
        public string FieldFilePath
        { get; set; }

        [XmlElement("TableStructFilePath")]
        public string TableStructFilePath
        { get; set; }
    }
}