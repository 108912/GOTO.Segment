using System.Collections.Generic;

namespace GOTO.BigDataAccess.DBSql.Model
{
    public class ConditionFieldModel
    {
        public ConditionFieldModel()
        {
            List = new List<ConditionFieldItemModel>();
        }
        public List<ConditionFieldItemModel> List;
    }
    public class ConditionFieldItemModel
    {
        public string FieldName
        { get; set; }
        public long ValueMin
        { get; set; }
        public long ValueMax
        { get; set; }
    }
}