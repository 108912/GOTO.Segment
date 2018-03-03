using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlDatabaseXml")]
    public class SqlDatabaseXml
    {
        [XmlElement("SqlDatabaseList")]
        public List<SqlDatabaseItemXml> SqlDatabaseList
        { get; set; }
    }
}