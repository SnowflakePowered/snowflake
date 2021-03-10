using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Romfile.Naming;
using Snowflake.Romfile.Tokenizer;
using Xunit;

namespace Snowflake.Romfile.Tests
{
    public class StructuredFilenameTests
    {
        [Theory]
        [InlineData(".hack G.U. Last Recode", ".hack G.U. Last Recode", NamingConvention.GoodTools, "ZZ")]
        [InlineData("Super Mario Bros. 2 (USA).nes", "Super Mario Bros. 2", NamingConvention.NoIntro, "US")]
        [InlineData("Super Mario Bros. 2 (USA)", "Super Mario Bros. 2", NamingConvention.NoIntro, "US")]
        [InlineData("Super Mario Bros. (USA)", "Super Mario Bros.", NamingConvention.NoIntro, "US")]
        [InlineData("Super Mario Bros.", "Super Mario Bros.", NamingConvention.GoodTools, "ZZ")]
        [InlineData(".hack_G.U. Last Recode.iso", ".hack_G.U. Last Recode", NamingConvention.GoodTools, "ZZ")]

        [InlineData("Super Mario Bros. (USA).nes", "Super Mario Bros.", NamingConvention.NoIntro, "US")]
        [InlineData("Super Mario Bros..nes", "Super Mario Bros.", NamingConvention.GoodTools, "ZZ")]

        [InlineData("RPG Maker Fes (Europe) (En,Fr,De,Es,It)", "RPG Maker Fes", NamingConvention.NoIntro, "EU")]
        [InlineData("Seisen Chronicle (Japan) (eShop) [b]", "Seisen Chronicle", NamingConvention.NoIntro, "JP")]
        [InlineData("Pachio-kun 3 (Japan) (Rev A)", "Pachio-kun 3", NamingConvention.NoIntro, "JP")]
        [InlineData("Barbie - Jet, Set & Style! (Europe) (De,Es,It) (VMZX) (NDSi Enhanced)",
            "Barbie - Jet, Set & Style!", NamingConvention.NoIntro, "EU")]
        [InlineData(
            "Barbie - Jet, Set & Style! (Europe) (De,Es,It) (Beta) (Proto) (Sample) (Unl) (VMZX) (NDSi Enhanced)",
            "Barbie - Jet, Set & Style!", NamingConvention.NoIntro, "EU")]
        [InlineData("Bart Simpson's Escape from Camp Deadly (USA, Europe).gb", "Bart Simpson's Escape from Camp Deadly",
            NamingConvention.NoIntro, "US-EU")]
        [InlineData("[BIOS] X'Eye (USA) (v2.00).md", "X'Eye", NamingConvention.NoIntro, "US")]
        [InlineData("[BIOS] X'Eye (USA) (v2.00) (En,Es,Fr).md", "X'Eye", NamingConvention.NoIntro, "US")]
        [InlineData("Adventures of Batman & Robin, The (USA)", "The Adventures of Batman & Robin",
            NamingConvention.NoIntro, "US")]
        [InlineData("Pokemon - Versione Blu (Italy) (SGB Enhanced).gb", "Pokemon - Versione Blu",
            NamingConvention.NoIntro, "IT")]
        [InlineData("Pop'n TwinBee (Europe).gb", "Pop'n TwinBee", NamingConvention.NoIntro, "EU")]
        [InlineData("Prince of Persia (Europe) (En,Fr,De,Es,It).gb", "Prince of Persia", NamingConvention.NoIntro,
            "EU")]
        [InlineData("Purikura Pocket 2 - Kareshi Kaizou Daisakusen (Japan) (SGB Enhanced).gb",
            "Purikura Pocket 2 - Kareshi Kaizou Daisakusen",
            NamingConvention.NoIntro, "JP")]
        [InlineData("Thrill Kill (1998-07-09)(Virgin)(proto).ccd", "Thrill Kill",
            NamingConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Biohazard 2 v2 (1997)(Capcom)(JP)(beta)", "Biohazard 2",
            NamingConvention.TheOldSchoolEmulationCenter, "JP")]
        [InlineData("Femten-Spill v1.2 (1986)(Datalauget)(NO)", "Femten-Spill",
            NamingConvention.TheOldSchoolEmulationCenter, "NO")]
        [InlineData("Alien 3 (1992)(BITS - LJN)(JP)(en)", "Alien 3", NamingConvention.TheOldSchoolEmulationCenter,
            "JP")]
        [InlineData("Aoki Densetsu Shoot! (J)", "Aoki Densetsu Shoot!", NamingConvention.GoodTools, "JP")]
        [InlineData("Bionic Commando (1992)(Capcom)(US)", "Bionic Commando",
            NamingConvention.TheOldSchoolEmulationCenter, "US")]
        [InlineData("Legend of TOSEC, The (demo) (1986)(Devstudio)", "The Legend of TOSEC",
            NamingConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Legend of TOSEC, The (demo) (19xx-08-09)(Devstudio)", "The Legend of TOSEC",
            NamingConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Legend of TOSEC, The (demo) (20xx-08-09)(Devstudio)", "The Legend of TOSEC",
            NamingConvention.TheOldSchoolEmulationCenter, "ZZ")]
        [InlineData("Legend of TOSEC, The (demo) (20xx-08-09)(Devstudio)(US-BR)", "The Legend of TOSEC",
            NamingConvention.TheOldSchoolEmulationCenter, "US-BR")]
        [InlineData("Legend of TOSEC, The (demo) (20xx-08-09)(Devstudio)(US-BR)(jp-yi)(NTSC)(Disc 1 of 1)[cr][!]",
            "The Legend of TOSEC", NamingConvention.TheOldSchoolEmulationCenter, "US-BR")]
        [InlineData("Legend of TOSEC, The (demo) (20xx-08-09)(Devstudio)(US-BR)(jp-yi)(NTSC)(Side A)[cr][!]",
            "The Legend of TOSEC", NamingConvention.TheOldSchoolEmulationCenter, "US-BR")]
        [InlineData("Guilty Gear Xrd Rev 2 (demo) (20xx-08-09)(Devstudio)(US-BR)(jp-yi)(NTSC)(Side A)[cr][!]",
            "Guilty Gear Xrd Rev 2", NamingConvention.TheOldSchoolEmulationCenter, "US-BR")]
        [InlineData("Liable Gear Xrd Rev 2 (demo) (20xx-08-09)(Devstudio)(US-BR)(jp-yi)(NTSC)(Side A)[cr][!]",
            "Liable Gear Xrd", NamingConvention.TheOldSchoolEmulationCenter, "US-BR")]
        [InlineData("Legend of Zelda, The - A Link to the Past (Canada) .zip",
            "The Legend of Zelda - A Link to the Past", NamingConvention.NoIntro, "CA")]
        [InlineData("Legend of Zelda, The - A Link to the Past (U) [!].zip", "The Legend of Zelda - A Link to the Past",
            NamingConvention.GoodTools, "US")]
        [InlineData("Legend of Zelda, The - A Link to the Past (U) [a?][!].zip",
            "The Legend of Zelda - A Link to the Past", NamingConvention.GoodTools, "US")]
        [InlineData("Legend of Zelda, The (U) (PRG 0).nes", "The Legend of Zelda", NamingConvention.GoodTools, "US")]
        [InlineData("Legend of Zelda, The (U) (NTSC) (8k) (M4) (PRG 0).nes", "The Legend of Zelda",
            NamingConvention.GoodTools, "US")]
        [InlineData("Legend of Zelda, The (U) (PAL) (32k) (PRG 0).nes", "The Legend of Zelda",
            NamingConvention.GoodTools, "US")]
        [InlineData("Legend of Zelda, The (1) (PRG 0).nes", "The Legend of Zelda", NamingConvention.GoodTools, "JP-KR")]
        [InlineData("Legend of Zelda, The (4) (PRG 0).nes", "The Legend of Zelda", NamingConvention.GoodTools, "US-BR")]
        [InlineData("Legend of GoodTools, The (Unl) (PRG 0).nes", "The Legend of GoodTools", NamingConvention.GoodTools,
            "ZZ")]
        [InlineData("Legend of Zelda, The (B) (PRG 0).nes", "The Legend of Zelda", NamingConvention.GoodTools, "BR")]
        public void StructuredFilename_Tests(string filename, string title, NamingConvention convention,
            string regioncode)
        {
            var structuredFilename = new StructuredFilename(filename);
            Assert.Equal(filename, structuredFilename.OriginalFilename);
            Assert.Equal(convention, structuredFilename.NamingConvention);
            Assert.Equal(regioncode, structuredFilename.RegionCode);
            Assert.Equal(title, structuredFilename.Title);
        }
    }
}
