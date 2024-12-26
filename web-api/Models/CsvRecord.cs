using CsvHelper.Configuration.Attributes;

namespace web_api.Models
{
    public class CsvRecord
    {

        public string TINKLAS { get; set; }
        public string OBJ_PAVADINIMAS { get; set; }
        public string OBJ_GV_TIPAS { get; set; }
        public string OBJ_NUMERIS { get; set; }
        public string PL_T { get; set; }

        [Name("P+")]
        public double? PPlus { get; set; }

        [Name("P-")]
        public double? PMinus { get; set; }
    }
}
