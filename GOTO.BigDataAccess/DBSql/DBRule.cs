using GOTO.BigDataAccess.DBSql.Model;
using GOTO.BigDataAccess.DBSql.XmlModel;
using System.Collections.Generic;
using System.Linq;

namespace GOTO.BigDataAccess.DBSql
{
    public class DBRule
    {
        public List<MatchServerList> GetMatchServer(SqlBaseItemXml basemodel,ConditionFieldModel conditionfield)
        {
            int matchnum = 0;
            List<MatchServerList> ServerList = new List<MatchServerList>();
            List<SqlFieldItemXml> fieldlist = new List<SqlFieldItemXml>();
            if (conditionfield != null && conditionfield.List != null && conditionfield.List.Count > 0)
            {
                List<SqlFieldItemXml> templist = new List<SqlFieldItemXml>();
                foreach (var condition in conditionfield.List)
                {
                    long valuemin = condition.ValueMin;
                    long valuemax = condition.ValueMax;
                    if (matchnum<1)
                    {
                        var tempfieldlist = DBConfig.GetFieldXmlConfig(basemodel).SqlFieldList.Where(m => m.FieldName == condition.FieldName).ToList();
                        foreach (var item in tempfieldlist)
                        {
                            if (GetMatchValid(item, condition.ValueMin, condition.ValueMax))
                            {
                                ServerList.Add(GetMatchObject(item));
                            }
                        }
                    }
                    else
                    {
                        int conditionvalidnum = 0;
                        foreach(var serverlist in ServerList.ToList())
                        {
                            var tempfield = DBConfig.GetFieldXmlConfig(basemodel).SqlFieldList.Where(m => m.TableNumber == serverlist.TableNumber && m.FieldName == condition.FieldName).ToList();
                            if (tempfield != null)
                            {
                                foreach (var itemfield in tempfield)
                                {
                                    if (GetMatchValid(itemfield, condition.ValueMin, condition.ValueMax))
                                    {
                                        conditionvalidnum++;
                                        break;
                                    }
                                }
                            }
                            if (conditionvalidnum < 1)
                            {
                                ServerList.Remove(serverlist);
                            }
                        }
                    }
                    
                    matchnum++;
                    
                }
            }

            return ServerList;
        }
        private bool GetMatchValid(SqlFieldItemXml item, long valuemin, long valuemax)
        {
            bool revalue = false;
            if ((item.ValueMin <= valuemin && item.ValueMax >= valuemin) || (item.ValueMin <= valuemax && item.ValueMax >= valuemax) ||
                (valuemin <= item.ValueMin && valuemax >= item.ValueMin) || (valuemin <= item.ValueMax && valuemax >= item.ValueMax))
            {
                revalue = true;
            }
            return revalue;
        }
        private MatchServerList GetMatchObject(SqlFieldItemXml item)
        {
            MatchServerList temp = new MatchServerList();
            temp.ServerNumber = item.ServerNumber;
            temp.DatabaseNumber = item.DatabaseNumber;
            temp.TableNumber = item.TableNumber;
            return temp;
        }
    }
}
