using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    /// <summary>
    /// A <see cref="IScrapeEngine{T}"/> encapsulates the management of multiple
    /// <see cref="IScrapeContext"/> and traversal over the resultant tree to produce
    /// a single object of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The resultant of this <see cref="IScrapeEngine{T}"/></typeparam>
    public interface IScrapeEngine<T>
    {
        /// <summary>
        /// Creates a job and returns its GUID in the engine.
        /// A reference to the created job is held until the result is produced.
        /// </summary>
        /// <param name="scrapers">The list of scrapers to use wtih this job.</param>
        /// <param name="cullers">The list of cullers to use with this job</param>
        /// <param name="initialSeeds">ANy initial seeds to begin with this job.</param>
        /// <returns>The GUID of the job.</returns>
        Guid CreateJob(IEnumerable<SeedTree> initialSeeds,
            IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers);

        /// <summary>
        /// Continues the job with the given GUID and return whether or not there is more
        /// work to be done.
        /// </summary>
        /// <param name="jobGuid">The GUID of the job.</param>
        /// <returns>A boolean indicating whether or not there is more work to be done with the job.</returns>
        Task<bool> ProceedJob(Guid jobGuid);

        /// <summary>
        /// Continues the job with the given GUID and return whether or not there is more
        /// work to be done.
        /// </summary>
        /// <param name="jobGuid">The GUID of the job.</param>
        /// <param name="initialSeeds">Seeds to add to the in-progress job.</param>
        /// <returns>A boolean indicating whether or not there is more work to be done with the job.</returns>
        Task<bool> ProceedJob(Guid jobGuid, IEnumerable<SeedContent> initialSeeds);

        /// <summary>
        /// Culls the seeds of a ScrapeJob.
        /// </summary>
        /// <param name="jobGuid">The GUID of the job</param>
        void CullJob(Guid jobGuid);

        /// <summary>
        /// Removes the given seeds, then culls the seeds of a ScrapeJob.
        /// </summary>
        /// <param name="jobGuid">The GUID of the job.</param>
        /// <param name="manualCull">The list of seeds to cull.</param>
        void CullJob(Guid jobGuid, IEnumerable<Guid> manualCull);

        /// <summary>
        /// Gets the current state of the seed tree of the context with the given job ID.
        /// </summary>
        /// <param name="jobGuid">The GUID of the job.</param>
        /// <returns>The list of seeds in the context of the given job.</returns>
        IEnumerable<ISeed> GetJobState(Guid jobGuid);

        /// <summary>
        /// Continues with the job until it is completed, return the result. and
        /// remove the job from the queue.
        /// </summary>
        /// <param name="jobGuid">The GUID of the job.</param>
        /// <returns>The result of the job.</returns>
        Task<T> Result(Guid jobGuid);
    }
}
