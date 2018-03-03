using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlTableXml")]
    public class SqlTableXml
    {
        [XmlElement("SqlTableList")]
        public List<SqlTableItemXml> SqlTableList
        { get; set; }
    }
}