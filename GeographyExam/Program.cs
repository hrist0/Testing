using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GeographyExam
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new GeographyEntities();
            var riversAndCountries = ctx.Rivers.Select(r => new
            {
                riverName = r.RiverName,
                riverLength = r.Length,
                countries = r.Countries
                .OrderBy(c => c.CountryName)
                .Select(c => c.CountryName)
            })
            .OrderByDescending(r => r.riverLength);

            var json = new JavaScriptSerializer().Serialize(riversAndCountries.ToList());

            File.WriteAllText("../../rivers.json", json);

        }
    }
}
