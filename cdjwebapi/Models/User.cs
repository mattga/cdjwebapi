//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace cdjwebapi.Models
{
    public class User : BaseModel
    {
    	public User()
    	{
    		Init();
    	}
    	
    	public User(CDJStatusCode code, string description = "") : base(code, description)
    	{
    		Init();
    	}
    
    	public User(Status status) : base(status)
    	{
    		Init();
    	}
    
        private void Init()
        {
            Rooms = new HashSet<Room>();
            RoomUsers = new HashSet<RoomUser>();
        }
    
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        public string spUsername { get; set; }
        public string spEmail { get; set; }
        public bool? spProduct { get; set; }
    
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<RoomUser> RoomUsers { get; set; }
    }
}
