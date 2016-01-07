using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cdjwebapi.Models
{
    public partial class Entities : DbContext
    {
        public Entities() : base(nameOrConnectionString: "db5cbd36be2b64435bb940a48d0153e78eEntities") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}