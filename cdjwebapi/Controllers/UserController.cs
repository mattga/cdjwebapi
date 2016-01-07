using System;
using System.Linq;
using System.Web.Http;
using cdjwebapi.Models;

namespace cdjwebapi.Controllers
{
    public class UserController : ApiController
    {
        // GET api/user
        public User[] Get()
        {
            try
            {
                using (var context = new Entities())
                {
                    return context.Users.ToArray();
                }
            }
            catch (Exception e)
            {
                return new User[]
                {
                    new Models.User { Status = new Status(e) }
                };
            }
        }

        // GET api/user/<id>
        public User Get(int id)
        {
            try
            {
                using (var context = new Entities())
                {
                    var res = (from u in context.Users
                               where u.UserId == id
                               select u);

                    if (res.Count() == 0)
                    {
                        return new User(StatusCode.NotFound);
                    }

                    return res.First();
                }
            }
            catch (Exception e)
            {
                return new User { Status = new Status(e) };
            }
        }

        // POST api/user
        public User Post(User u)
        {
            try
            {
                using (var context = new Entities())
                {
                    u.CreatedDate = DateTime.Now;
                    context.Users.Add(u);
                    context.SaveChanges();

                    return u;
                }
            }
            catch (Exception e)
            {
                return new User { Status = new Status(e) };
            }
        }
    }
}
