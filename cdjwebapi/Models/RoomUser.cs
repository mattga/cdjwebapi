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
    public class RoomUser : BaseModel
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public long Tokens { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
