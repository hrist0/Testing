using GeographyExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExportFromXML
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new GeographyEntities();
            var countries = ctx.Countries.Where(m => m.Monasteries.Count() > 0).Select(c => new
            {
                country = c.CountryName,
                monastery = c.Monasteries.OrderBy(m => m.Name).Select(m => m.Name)
            })
            .OrderBy(c => c.country);

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            var body = doc.CreateElement("monasteries");
            doc.AppendChild(body);

            foreach (var item in countries)
            {
                var country = doc.CreateElement("country");
                country.SetAttribute("name", item.country);
                body.AppendChild(country);

                foreach (var monk in item.monastery)
                {
                    var mon = doc.CreateElement("monastery");
                    mon.InnerText = monk;
                    country.AppendChild(mon);
                }
            }
            doc.Save("monasteries.xml");
        }
    }
}
