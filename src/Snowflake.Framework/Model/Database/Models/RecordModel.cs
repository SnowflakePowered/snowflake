using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    internal class RecordModel
    {
        public Guid RecordID { get; set; }

        public string RecordType { get; set; }

        public List<RecordMetadataModel> Metadata { get; set; }
    }
}
