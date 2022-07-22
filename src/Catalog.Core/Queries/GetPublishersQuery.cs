﻿using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetPublishersQuery() : IRequest<IEnumerable<Publisher>>;
}
