using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snowflake.Core;
using Snowflake.Information.Platform;
using Snowflake.Information.Game;
using Snowflake.Plugin.Interface;
using Snowflake.Plugin.Scraper;
using Newtonsoft.Json;
using Snowflake.Constants;
using System.Threading;
using Snowflake.Extensions;
using DuoVia.FuzzyStrings;

namespace Snowflake.Core.Init
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FrontendCore.InitCore();
            Init();
        }

        async Task Init()
        {
            await FrontendCore.InitPluginManagerAsync();
           
            var dbgame = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID("sWJznptYf0m_qH0_OvHtSg");
            this.textBox1.Text = JsonConvert.SerializeObject(dbgame);
        }
    }
}
