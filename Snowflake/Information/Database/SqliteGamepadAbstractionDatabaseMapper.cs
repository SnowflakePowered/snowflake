using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;
using Snowflake.Controller;
using Snowflake.Game;

namespace Snowflake.Information.Database
{
    internal class SqliteGamepadAbstractionDatabaseMapper : ClassMapper<GamepadAbstraction>
    {
        public SqliteGamepadAbstractionDatabaseMapper()
        {
            base.Table("gamepadabstraction");
            this.Map(abs => abs.DeviceName).Key(KeyType.Assigned).Column("DeviceName");
            this.Map(abs => abs.ProfileType).Column("ProfileType");

            this.Map(abs => abs.A).Column("btnA");
            this.Map(abs => abs.B).Column("btnB");
            this.Map(abs => abs.X).Column("btnX");
            this.Map(abs => abs.Y).Column("btnY");
            this.Map(abs => abs.Start).Column("btnStart");
            this.Map(abs => abs.Select).Column("btnSelect");

            this.Map(abs => abs.DpadUp).Column("DpadUp");
            this.Map(abs => abs.DpadDown).Column("DpadDown");
            this.Map(abs => abs.DpadLeft).Column("DpadLeft");
            this.Map(abs => abs.DpadRight).Column("DpadRight");

            this.Map(abs => abs.RightAnalogXLeft).Column("RightAnalogXLeft");
            this.Map(abs => abs.RightAnalogXRight).Column("RightAnalogXRight");
            this.Map(abs => abs.RightAnalogYUp).Column("RightAnalogYUp");
            this.Map(abs => abs.RightAnalogYDown).Column("RightAnalogYDown");

            this.Map(abs => abs.LeftAnalogXLeft).Column("LeftAnalogXLeft");
            this.Map(abs => abs.LeftAnalogXRight).Column("LeftAnalogXRight");
            this.Map(abs => abs.LeftAnalogYUp).Column("LeftAnalogYUp");
            this.Map(abs => abs.LeftAnalogYDown).Column("LeftAnalogYDown");

            this.Map(abs => abs.L1).Column("L1");
            this.Map(abs => abs.L2).Column("L2");
            this.Map(abs => abs.L3).Column("L3");

            this.Map(abs => abs.R1).Column("R1");
            this.Map(abs => abs.R2).Column("R2");
            this.Map(abs => abs.R3).Column("R3");
        }
    }
    internal class SqliteIGamepadAbstractionDatabaseMapper : ClassMapper<IGamepadAbstraction>
    {
        public SqliteIGamepadAbstractionDatabaseMapper()
        {
            base.Table("gamepadabstraction");
            this.Map(abs => abs.DeviceName).Key(KeyType.Assigned).Column("DeviceName");
            this.Map(abs => abs.ProfileType).Column("ProfileType");

            this.Map(abs => abs.A).Column("btnA");
            this.Map(abs => abs.B).Column("btnB");
            this.Map(abs => abs.X).Column("btnX");
            this.Map(abs => abs.Y).Column("btnY");
            this.Map(abs => abs.Start).Column("btnStart");
            this.Map(abs => abs.Select).Column("btnSelect");

            this.Map(abs => abs.DpadUp).Column("DpadUp");
            this.Map(abs => abs.DpadDown).Column("DpadDown");
            this.Map(abs => abs.DpadLeft).Column("DpadLeft");
            this.Map(abs => abs.DpadRight).Column("DpadRight");

            this.Map(abs => abs.RightAnalogXLeft).Column("RightAnalogXLeft");
            this.Map(abs => abs.RightAnalogXRight).Column("RightAnalogXRight");
            this.Map(abs => abs.RightAnalogYUp).Column("RightAnalogYUp");
            this.Map(abs => abs.RightAnalogYDown).Column("RightAnalogYDown");

            this.Map(abs => abs.LeftAnalogXLeft).Column("LeftAnalogXLeft");
            this.Map(abs => abs.LeftAnalogXRight).Column("LeftAnalogXRight");
            this.Map(abs => abs.LeftAnalogYUp).Column("LeftAnalogYUp");
            this.Map(abs => abs.LeftAnalogYDown).Column("LeftAnalogYDown");

            this.Map(abs => abs.L1).Column("L1");
            this.Map(abs => abs.L2).Column("L2");
            this.Map(abs => abs.L3).Column("L3");

            this.Map(abs => abs.R1).Column("R1");
            this.Map(abs => abs.R2).Column("R2");
            this.Map(abs => abs.R3).Column("R3");
        }
    }
}
