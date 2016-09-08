using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ChemCloud_EmailSetting : BaseModel
    {
        private long _id;
        public new long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }

        //,SmtpServer
        public string SmtpServer { get; set; }
        //,SendMailId
        public string SendMailId { get; set; }
        //,SendMailPassword
        public string SendMailPassword { get; set; }
    }
}
