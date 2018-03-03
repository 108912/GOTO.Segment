using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlFieldXml")]
    public class SqlFieldXml
    {
        [XmlElement("SqlFieldList")]
        public List<SqlFieldItemXml> SqlFieldList
        { get; set; }
    }
}