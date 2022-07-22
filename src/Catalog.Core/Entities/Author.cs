﻿using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Author : EntityBase, IAggregateRoot
    {
        public Author(string name) : base()
        {
            Name = name;
            Books = new HashSet<Book>();
        }

        public string Name { get; private set; }

        public ICollection<Book> Books { get; private set; }
    }
}
