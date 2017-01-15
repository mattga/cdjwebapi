using cdjwebapi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace cdjwebapi.Controllers
{
    [RoutePrefix("api/room")]
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
                    var room = context.Rooms.Find(id);

                    if (room == null)
                    {
                        return new Room(CDJStatusCode.NotFound);
                    }

                    room.RoomTracks = context.RoomTracks
                        .Where(rt => rt.RoomId == room.RoomId)
                        .OrderByDescending(rt => rt.Tokens)
                        .ToList<RoomTrack>(); // Load songs (sorted)
                    context.Entry(room).Reference(r => r.Host).Load(); // Load host
                    context.Entry(room).Collection(r => r.RoomUsers).Load(); // Load users
                    foreach (RoomUser roomUser in room.RoomUsers)
                    {
                        context.Entry(roomUser).Reference(ru => ru.User).Load(); // Load each user
                    }

                    return room;
                }
            }
            catch (Exception e)
            {
                return new Room { Status = new Status(e) };
            }
        }

        // POST api/room/<id>/join
        [Route("{id:int}/join"), HttpPost]
        public RoomUser Join(int id, User u)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = (from ru in context.RoomUsers
                               where ru.RoomId == id & ru.UserId == u.UserId
                               select ru);

                    RoomUser roomUser;
                    if (res.Count() > 0)
                    {
                        roomUser = res.First();
                        roomUser.Status = new Status(CDJStatusCode.Exists, "RoomUser already exists");
                        return roomUser;
                    }

                    roomUser = new RoomUser();
                    roomUser.UserId = u.UserId;
                    roomUser.RoomId = id;
                    roomUser.Tokens = 100;
                    context.RoomUsers.Add(roomUser);

                    int ret = context.SaveChanges();

                    if (ret > 0) return roomUser;
                    else return new RoomUser {
                        Status = new Status(CDJStatusCode.Error, "Failed to commit new RoomUser")
                    };
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new RoomUser { Status = new Status(e) };
            }
        }

        // POST api/room/<id>/addtrack
        [Route("{rid:int}/user/{uid:int}/addtrack"), HttpPost]
        public RoomTrack AddSong(int rid, int uid, RoomTrack t)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = (from rt in context.RoomTracks
                               where rt.RoomId == rid & rt.SourceId == t.SourceId
                               select rt);

                    RoomTrack track;
                    if (res.Count() > 0)
                    {
                        track = res.First();
                        track.Tokens += t.Tokens;
                        track.Status = new Status(CDJStatusCode.Exists, "RoomSong already exists. Added tokens.");
                    }
                    else
                    {
                        track = t;
                        track.RoomId = rid;
                        track.PublishedDate = DateTime.Now;
                        track.Status = new Status(); // OK

                        context.RoomTracks.Add(track);
                    }

                    RoomUser roomUser = context.RoomUsers
                        .Where(ru => ru.RoomId == rid && ru.UserId == uid)
                        .First();
                    roomUser.Tokens -= t.Tokens;

                    int ret = context.SaveChanges();

                    if (ret > 0) return track;
                    else return new RoomTrack
                    {
                        Status = new Status(CDJStatusCode.Error, "Failed to commit RoomSong")
                    };
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new RoomTrack { Status = new Status(e) };
            }
        }
    }
}
