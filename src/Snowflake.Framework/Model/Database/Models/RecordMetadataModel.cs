using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    internal class RecordMetadataModel
    {
        public Guid RecordMetadataID { get; set; }
        public Guid RecordID { get; set; }
        public RecordModel? Record { get; set; }

        public string MetadataKey { get; set; }
        public string MetadataValue { get; set; } = "";
    }
}
