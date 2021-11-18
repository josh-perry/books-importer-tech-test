using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace TechTest
{
    public class BooksImport
    {
        public void Import()
        {
            // download books xml from endpoint
            string xml;
            using (var client = new WebClient())
            {
                var url = "https://www.w3schools.com/xml/books.xml";
                xml = client.DownloadString(url);
            }

            // parse books xml to a books collection
            var document = XDocument.Parse(xml);
            var books = (from b in document.Descendants("book")
                select new
                {
                    Category = b.Attribute("category").Value,
                    Title = b.Element("title").Value,
                    Authors = b.Elements("author").Select(e => e.Value),
                    Price = decimal.Parse(b.Element("price").Value)
                });

            // save books to the database
            using (var conn = new SqlConnection("connString"))
            using (var comm = conn.CreateCommand())
            {
                using (var trans = conn.BeginTransaction())
                {
                    foreach (var b in books)
                    {
                        // build sql or call sp to insert book
                        comm.ExecuteNonQuery();
                    }

                    trans.Commit();
                }
            }
        }
    }
}
