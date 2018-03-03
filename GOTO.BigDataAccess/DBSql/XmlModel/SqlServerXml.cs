using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlServerXml")]
    public class SqlServerXml
    {
        [XmlElement("SqlServerList")]
        public List<SqlServerItemXml> SqlServerList
        { get; set; }
    }
}