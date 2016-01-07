using System;

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
            this.status = new Status(code);
        }
    }

    public class Status
    {
        public StatusCode Code { get; set; }
        public string Description { get; set; }
        public string StackTrace { get; set; }

        public Status(StatusCode code = StatusCode.Ok, string description = "", 
            string stackTrace = "")
        {
            this.Code = code;
            this.Description = Description;
            this.StackTrace = StackTrace;
        }

        public Status(Exception e)
        {
            this.Code = StatusCode.Error;
            this.Description = e.Message;
            this.StackTrace = e.StackTrace;
        }
    }
}