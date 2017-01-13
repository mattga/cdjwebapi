﻿using cdjwebapi.Models;
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

                    room.RoomSongs = context.RoomSongs
                        .Where(rs => rs.RoomId == room.RoomId)
                        .OrderByDescending(rs => rs.Tokens)
                        .ToList<RoomSong>(); // Load songs (sorted)
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

        // POST api/room/<id>/addsong
        [Route("{rid:int}/user/{uid:int}/addsong"), HttpPost]
        public RoomSong AddSong(int rid, int uid, RoomSong s)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = (from rs in context.RoomSongs
                               where rs.RoomId == rid & rs.SourceId == s.SourceId
                               select rs);

                    RoomSong roomSong;
                    if (res.Count() > 0)
                    {
                        roomSong = res.First();
                        roomSong.Tokens += s.Tokens;
                        roomSong.Status = new Status(CDJStatusCode.Exists, "RoomSong already exists. Added tokens.");
                    }
                    else
                    {
                        roomSong = s;
                        roomSong.RoomId = rid;
                        roomSong.PublishedDate = DateTime.Now;
                        roomSong.Status = new Status(); // OK

                        context.RoomSongs.Add(roomSong);
                    }

                    RoomUser roomUser = context.RoomUsers
                        .Where(ru => ru.RoomId == rid && ru.UserId == uid)
                        .First();
                    roomUser.Tokens -= s.Tokens;

                    int ret = context.SaveChanges();

                    if (ret > 0) return roomSong;
                    else return new RoomSong
                    {
                        Status = new Status(CDJStatusCode.Error, "Failed to commit RoomSong")
                    };
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new RoomSong { Status = new Status(e) };
            }
        }
    }
}
