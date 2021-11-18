using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace TechTest
{
    public class BooksImporter : IBooksImporter
    {
        /// <summary>
        /// Download XML source data for the books from the given URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string DownloadBooksXml(string url)
        {
            using var webClient = new WebClient();
            return webClient.DownloadString(url);
        }

        /// <summary>
        /// Retrieve a list of book objects from the supplied XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IEnumerable<Book> ParseBooks(string xml)
        {
            var document = XDocument.Parse(xml);

            // NOTE: Deserialize into a list of concrete Book objects instead, also handle null fields more elegantly.
            //       If this were to get significantly larger we could consider moving this out into a mapper class.
            var books = document.Descendants("book")
                .Select(b => new Book
                {
                    Category = b.Attribute("category")?.Value,
                    Title = b.Element("title")?.Value,
                    Authors = b.Elements("author").Select(e => e.Value),
                    Price = decimal.Parse(b.Element("price")?.Value ?? string.Empty),
                    Year = int.Parse(b.Element("year")?.Value ?? string.Empty)
                });

            return books.ToList();
        }

        private void SaveBooks(IEnumerable<Book> books)
        {
            // NOTE: This connection string should come from a config/environment variable
            using var sqlConnection = new SqlConnection("connString");
            using var sqlCommand = sqlConnection.CreateCommand();
            using var transaction = sqlConnection.BeginTransaction();

            foreach (var b in books)
            {
                // Build sql or call sp to insert book
                sqlCommand.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        /// <summary>
        /// Imports a set of books into the database
        /// </summary>
        public void Import()
        {
            // NOTE: This URL should definitely come from a config of some kind: either passed into this class or
            //       the config manager should be injected here and the URL fetched from that.
            var xml = DownloadBooksXml("https://www.w3schools.com/xml/books.xml");
            var books = ParseBooks(xml);
            SaveBooks(books);
        }
    }
}
