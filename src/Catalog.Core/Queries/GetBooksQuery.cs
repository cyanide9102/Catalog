﻿using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetBooksQuery() : IRequest<IEnumerable<Book>>;
}
