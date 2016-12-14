using System;
using System.Runtime.Serialization;

namespace cdjwebapi.Models
{
    public enum CDJStatusCode
    {
        Ok = 0,
        NotFound,
        Exists,
        Timeout,
        NoAuth,
        Error = 99
    }

    [Serializable]
    public class BaseModel
    {
        [DataMember]
        private Status status = new Status();

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

        public BaseModel(CDJStatusCode code = CDJStatusCode.Ok, string description = "")
        {
            status = new Status(code, description);
        }
        
        public BaseModel(Status status)
        {
            this.status = status;
        }
    }

    public class Status
    {
        public CDJStatusCode Code { get; set; }
        public string Description { get; set; }
        public string StackTrace { get; set; }
        
        public Status(CDJStatusCode code = CDJStatusCode.Ok, string description = "",
            string stackTrace = "")
        {
            Code = code;
            Description = description;
            StackTrace = stackTrace;
        }

        public Status(Exception e)
        {
            Code = CDJStatusCode.Error;
            Description = e.Message;
            StackTrace = e.StackTrace;
        }
    }
}