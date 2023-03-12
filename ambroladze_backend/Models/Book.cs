﻿namespace ambroladze_backend.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        //public Book(int id) { }
        public Book(string Title, string Author)
        {
            this.Author = Author;
            this.Title = Title;
        }
    }
}
