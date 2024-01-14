using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data;

namespace Infrastructure.Seeds
{
    public interface IDbContextSeed
    {
        public Task SeedAsync(ToDoContext context);
    }
}
