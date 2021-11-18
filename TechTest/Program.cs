using System.Net;

namespace TechTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var booksImport = new BooksImport();
            booksImport.Import();
        }
    }
}
