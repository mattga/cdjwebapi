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
                using (var context = new DbEntities())
                {
                    var a = context.Users.ToArray();
                    return a;
                }
            }
            catch (Exception e)
            {
                return new User[]
                {
                    new User { Status = new Status(e) }
                };
            }
        }

        // GET api/user/<id>
        public User Get(int id)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = context.Users.SingleOrDefault(u => u.UserId == id);

                    if (res == null)
                    {
                        return new User(StatusCode.NotFound);
                    }

                    return res;
                }
            }
            catch (Exception e)
            {
                return new User { Status = new Status(e) };
            }
        }

        // POST api/user
        public User Post(User user)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    user.CreatedDate = DateTime.Now;
                    context.Users.Add(user);
                    context.SaveChanges();

                    return user;
                }
            }
            catch (Exception e)
            {
                return new User(new Status(e));
            }
        }

        // PUT api/user
        public User Put(User user)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = context.Users.SingleOrDefault(u => u.spEmail == user.spEmail);

                    if (res == null)
                    {
                        res = context.Users.Add(user);
                        res.CreatedDate = DateTime.Now;
                        res.Status = new Status(StatusCode.NotFound);
                    }
                    else
                    {
                        res.spEmail = user.spEmail;
                        res.spUsername = user.spUsername;
                        res.spProduct = user.spProduct;
                        res.ImageUrl = user.ImageUrl;
                    }
                    context.SaveChanges();

                    return res;
                }
            }
            catch (Exception e)
            {
                return new User(new Status(e));
            }
        }

        // POST api/user/authenticate
        [ActionName("authenticate")]
        public User Authenticate(User user)
        {
            try
            {
                using (var context = new DbEntities())
                {
                    var res = context.Users.SingleOrDefault(u => 
                        u.Username == user.Username &&
                        u.Password == user.Password);

                    if (res == null)
                    {
                        return new User(new Status(StatusCode.NoAuth));
                    }

                    return res;
                }
            }
            catch (Exception e)
            {
                return new User(new Status(e))
;
            }
        }
    }
}
