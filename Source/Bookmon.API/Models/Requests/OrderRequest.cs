using System;
using System.Collections.Generic;

namespace Bookmon.API.Models.Requests;

public sealed class OrderRequest
{
    public IList<Guid> Books { get; set; }
}