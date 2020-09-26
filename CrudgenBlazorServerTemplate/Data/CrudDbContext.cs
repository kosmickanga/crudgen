using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerWeb_CSharp.Data
{
    public class CrudDbContext : DbContext
    {
        public CrudDbContext(DbContextOptions<CrudDbContext> options)
            : base(options)
        {
            // will be overwritten by generated code.
        }
    }
}
