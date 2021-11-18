using System.Collections.Generic;

namespace TechTest
{
    public class Book
    {
        public string Category { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<string> Authors { get; set; }
    }
}
