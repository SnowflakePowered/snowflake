using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Romfile
{ 
    /// <summary>
    /// Represents a structured filename of a ROM according to any of either NoIntro, TOSEC or GoodTools conventions.
    /// Any other data besides title, year, and region is discarded.
    /// </summary>
    public interface IStructuredFilename
    {
        /// <summary>
        /// The naming convention of the filename.
        /// </summary>
        /// <remarks>
        /// This value is determined by the format of the filename's country (region code) (NOT LANGUAGE)
        /// TOSEC uses ISO 3166-1 alpha-2 2-letter country codes seperated by a hyphen.
        /// GoodTools uses it's own 1 or 2 letter format
        /// No-Intro uses full-country names that seem to follow their own conventions. 
        /// </remarks>
        StructuredFilenameConvention NamingConvention { get; }
        /// <summary>
        /// The title of the game according to the filename.
        /// May or may not differ from the final queryable filename.
        /// This title is always in ASCII. Japanese and other languages are romanized in accordance with the filename's convention.
        /// Version information is discarded.
        /// </summary>
        string Title { get; }
        /// <summary>
        /// The year of the ROM (0 if No-Intro)
        /// </summary>
        /// <remarks>
        /// Unreliable as the No-Intro convention does not mandate a year.
        /// Begins with either 19XX or 20XX if TOSEC or GoodTools.
        /// This is a string as unknown dates are serialized as 19XX or 20XX.
        /// Does not contain any other date information such as month and day, this data is discarded
        /// </remarks>
        string Year { get; }
        /// <summary>
        /// The region code of the game in format ISO 3166-1 alpha-2 2-letter country codes seperated by a hyphen.
        /// </summary>
        /// <remarks>
        /// This is converted from whatever format the original naming convention uses. TOSEC values are taken verbatim if under 2 codes,
        /// while No-Intro and GoodTools are converted via lookup=table.
        /// 
        /// If TOSEC values use more than 3 codes, or if the value for GoodTools is W, Unl, PD, or Unk, or if the No-Intro value is World or Unknown,
        /// this value is serialized into the ISO code 'ZZ' for Unknown or Invalid Territory.
        /// </remarks>
        string RegionCode { get; }

    }

    /// <summary>
    /// Types of filename conventions
    /// </summary>
    public enum StructuredFilenameConvention
    {
        /// <summary>
        /// No-Intro Naming Convention
        /// </summary>
        NoIntro,
        /// <summary>
        /// TOSEC Naming Convention
        /// </summary>
        TheOldSchoolEmulationCenter,
        /// <summary>
        /// GoodTools naming convention
        /// </summary>
        GoodTools
    }

}
