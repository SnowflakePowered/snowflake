using Snowflake.Romfile.Tokenizer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowflake.Romfile.Tests
{
    public class StructuredFilenameTests
    {
        [Theory]
        [InlineData("Bart Simpson's Escape from Camp Deadly (USA, Europe).gb", "Bart Simpson's Escape from Camp Deadly", NamingConvention.NoIntro, "US-EU")]
        [InlineData("[BIOS] X'Eye (USA) (v2.00).md", "X'Eye", NamingConvention.NoIntro, "US")]
        [InlineData("Adventures of Batman & Robin, The (USA)", "The Adventures of Batman & Robin", NamingConvention.NoIntro, "US")]
        [InlineData("Pokemon - Versione Blu (Italy) (SGB Enhanced).gb", "Pokemon - Versione Blu", NamingConvention.NoIntro, "IT")]
        [InlineData("Pop'n TwinBee (Europe).gb", "Pop'n TwinBee", NamingConvention.NoIntro, "EU")]
        [InlineData("Prince of Persia (Europe) (En,Fr,De,Es,It).gb", "Prince of Persia", NamingConvention.NoIntro, "EU")]
        [InlineData("Purikura Pocket 2 - Kareshi Kaizou Daisakusen (Japan) (SGB Enhanced).gb", "Purikura Pocket 2 - Kareshi Kaizou Daisakusen", 
            NamingConvention.NoIntro, "JP")]
        [InlineData("Thrill Kill (1998-07-09)(Virgin)(proto).ccd", "Thrill Kill", NamingConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Biohazard 2 v2 (1997)(Capcom)(JP)(beta)", "Biohazard 2", NamingConvention.TheOldSchoolEmulationCenter, "JP")]
        [InlineData("Femten-Spill v1.2 (1986)(Datalauget)(NO)", "Femten-Spill", NamingConvention.TheOldSchoolEmulationCenter, "NO")]
        [InlineData("Alien 3 (1992)(BITS - LJN)(JP)(en)", "Alien 3", NamingConvention.TheOldSchoolEmulationCenter, "JP")]
        [InlineData("Aoki Densetsu Shoot! (J)", "Aoki Densetsu Shoot!", NamingConvention.GoodTools, "JP")]
        [InlineData("Bionic Commando (1992)(Capcom)(US)", "Bionic Commando", NamingConvention.TheOldSchoolEmulationCenter, "US")]
        [InlineData("Legend of TOSEC, The (demo) (1986)(Devstudio)", "The Legend of TOSEC", NamingConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Legend of Zelda, The - A Link to the Past (1970) (USA)", "The Legend of Zelda - A Link to the Past", NamingConvention.NoIntro, "US")]
        public void StructuredFilename_Tests(string filename, string title, NamingConvention convention, string regioncode)
        {
            var structuredFilename = new StructuredFilename(filename);
            Assert.Equal(filename, structuredFilename.OriginalFilename);
            Assert.Equal(convention, structuredFilename.NamingConvention);
            Assert.Equal(regioncode, structuredFilename.RegionCode);
            Assert.Equal(title, structuredFilename.Title);
        }
    }
}
