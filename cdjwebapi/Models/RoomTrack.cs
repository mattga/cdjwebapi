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
    public class RoomTrack : BaseModel
    {
    	public RoomTrack()
    	{
    		Init();
    	}
    	
    	public RoomTrack(CDJStatusCode code, string description = "") : base(code, description)
    	{
    		Init();
    	}
    
    	public RoomTrack(Status status) : base(status)
    	{
    		Init();
    	}
    
        private void Init()
        {
            Rooms = new HashSet<Room>();
        }
    
        public int TrackId { get; set; }
        public int RoomId { get; set; }
        public string SourceId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Thumbnail { get; set; }
        public string Thumbnail_Lg { get; set; }
        public int? Length { get; set; }
        public System.DateTime? PublishedDate { get; set; }
        public int Tokens { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string Source { get; set; }
        public int? SourceLikeCount { get; set; }
    
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual Room Room { get; set; }
    }
}
