using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Models
{
    public class MessageModel
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Info,
        Warning,
        Error
    }
}