using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowflake.Romfile.Tests
{
    public class StructuredFilenameTests
    {
        [Theory]
        [InlineData("Bart Simpson's Escape from Camp Deadly (USA, Europe).gb", "Bart Simpson's Escape from Camp Deadly", StructuredFilenameConvention.NoIntro, "US-EU")]
        [InlineData("[BIOS] X'Eye (USA) (v2.00).md", "X'Eye", StructuredFilenameConvention.NoIntro, "US")]
        [InlineData("Adventures of Batman & Robin, The (USA)", "The Adventures of Batman & Robin", StructuredFilenameConvention.NoIntro, "US")]
        [InlineData("Pokemon - Versione Blu (Italy) (SGB Enhanced).gb", "Pokemon - Versione Blu", StructuredFilenameConvention.NoIntro, "IT")]
        [InlineData("Pop'n TwinBee (Europe).gb", "Pop'n TwinBee", StructuredFilenameConvention.NoIntro, "EU")]
        [InlineData("Prince of Persia (Europe) (En,Fr,De,Es,It).gb", "Prince of Persia", StructuredFilenameConvention.NoIntro, "EU")]
        [InlineData("Purikura Pocket 2 - Kareshi Kaizou Daisakusen (Japan) (SGB Enhanced).gb", "Purikura Pocket 2 - Kareshi Kaizou Daisakusen", 
            StructuredFilenameConvention.NoIntro, "JP")]
        //[InlineData("Thrill Kill (1998-07-09)(Virgin)(proto).ccd", "Thrill Kill", StructuredFilenameConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Biohazard 2 v2 (1997)(Capcom)(JP)(beta)", "Biohazard 2 v2", StructuredFilenameConvention.TheOldSchoolEmulationCenter, "JP")]
        //[InlineData("Femten-Spill v1.2 (1986)(Datalauget)(NO)", "Femten-Spill v1.2", StructuredFilenameConvention.TheOldSchoolEmulationCenter, "NO")]
        [InlineData("Alien 3 (1992)(BITS - LJN)(JP)(en)", "Alien 3", StructuredFilenameConvention.TheOldSchoolEmulationCenter, "JP")]
        [InlineData("Aoki Densetsu Shoot! (J)", "Aoki Densetsu Shoot!", StructuredFilenameConvention.GoodTools, "JP")]
        [InlineData("Bionic Commando (1992)(Capcom)(US)", "Bionic Commando", StructuredFilenameConvention.TheOldSchoolEmulationCenter, "US")]
        public void StructuredFilename_Tests(string filename, string title, StructuredFilenameConvention convention, string regioncode)
        {
            var structuredFilename = new StructuredFilename(filename);
            Assert.Equal(filename, structuredFilename.OriginalFilename);
            Assert.Equal(convention, structuredFilename.NamingConvention);
            Assert.Equal(regioncode, structuredFilename.RegionCode);
            Assert.Equal(title, structuredFilename.Title);
        }
    }
}
