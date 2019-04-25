using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// Specifies the requirements of a directive evaluated when
    /// deciding if a <see cref="IScraper"/> is run or not.
    /// </summary>
    public enum Directive
    {
        /// <summary>
        /// Indicates that a child of the specified type on the specified <see cref="AttachTarget"/>
        /// MUST exist before this scraper is run. Seed types of this directive are guaranteed to
        /// be available when this scraper is run.
        /// </summary>
        Requires,

        /// <summary>
        /// Indicates that a child of the specified type on the specified <see cref="AttachTarget"/>
        /// MUST NOT exist before this scraper is run. Allows for fallbacks and exclusions on multiple but
        /// similar scrapers that can take advantage of pre-existing data.
        /// All scrapers are <em>eager</em> to run, and will run immediately after their <see cref="Requires"/>
        /// are fulfilled. That means that exclusions apply only to seeds that exist on the previous seed state,
        /// before this scraper is run. In order words, if the <see cref="Requires"/> of a scraper is fulfilled
        /// before all possible <see cref="Excludes"/> may have appeared, the scraper will run regardless of any
        /// <see cref="Excludes"/>.
        /// If the state of the seed tree excludes this scraper after it has already been run,
        /// the result will exist for the remainder of the <see cref="IScrapeContext"/> unless it is
        /// culled by an <see cref="ICuller"/>.
        /// </summary>
        Excludes,
    }
}
