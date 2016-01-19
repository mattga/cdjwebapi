using System;
using System.Runtime.Serialization;

namespace cdjwebapi.Models
{
    public enum StatusCode
    {
        Ok = 0,
        NotFound,
        Exists,
        Timeout,
        NoAuth,
        Error = 99
    }

    public class BaseModel
    {
        private Status status = new Status();

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

        public BaseModel(StatusCode code = StatusCode.Ok)
        {
            status = new Status(code);
        }

        public BaseModel(Status status)
        {
            this.status = status;
        }
    }

    [DataContract]
    public class Status
    {
        [DataMember]
        public StatusCode Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string StackTrace { get; set; }

        public Status() { }

        public Status(StatusCode code = StatusCode.Ok, string description = "",
            string stackTrace = "")
        {
            Code = code;
            Description = Description;
            StackTrace = StackTrace;
        }

        public Status(Exception e)
        {
            Code = StatusCode.Error;
            Description = e.Message;
            StackTrace = e.StackTrace;
        }
    }
}