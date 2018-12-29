using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Game;

namespace Snowflake.Model.Database.Models
{
    internal class FileRecordModel : RecordModel
    {
        public string MimeType { get; set; } = "application/octet-stream";
    }
}
