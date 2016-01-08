using cdjwebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cdjwebapi.Controllers
{
    public class RoomController : ApiController
    {
        // GET api/room
        public Room[] Get()
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var rs = context.Rooms.ToArray();
                    return rs;
                }
            }
            catch (Exception e)
            {
                return new Room[]
                {
                    new Room { Status = new Status(e) }
                };
            }
        }

        // GET api/room/<id>
        public Room Get(int id)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = (from r in context.Rooms
                               where r.RoomId == id
                               select r);

                    if (res.Count() == 0)
                    {
                        return new Room(StatusCode.NotFound);
                    }

                    var room = res.First();
                    context.Entry(room).Reference(r => r.Host).Load();      // Load host
                    context.Entry(room).Collection(r => r.RoomUsers).Load(); // Load users
                    context.Entry(room).Collection(r => r.RoomSongs).Load(); // Load songs

                    return room;
                }
            }
            catch (Exception e)
            {
                return new Room { Status = new Status(e) };
            }
        }
    }
}
