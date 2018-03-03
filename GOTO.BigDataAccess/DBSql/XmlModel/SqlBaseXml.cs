using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GOTO.BigDataAccess.DBSql.XmlModel
{
    [Serializable]
    [XmlRoot("SqlBaseXml")]
    public class SqlBaseXml
    {
        [XmlElement("SqlBaseList")]
        public List<SqlBaseItemXml> SqlBaseList
        { get; set; }
    }
}