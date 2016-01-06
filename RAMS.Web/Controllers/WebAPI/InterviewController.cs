﻿using RAMS.Helpers;
using RAMS.Models;
using RAMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RAMS.Web.Controllers.WebAPI
{
    /// <summary>
    /// InterviewController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class InterviewController : ApiController
    {
        private readonly IInterviewService InterviewService;

        /// <summary>
        /// Controller that sets interview service in order to access context resources
        /// </summary>
        /// <param name="interviewService">Parameter for setting interview service</param>
        public InterviewController(IInterviewService interviewService)
        {
            this.InterviewService = interviewService;
        }

        /// <summary>
        /// Get the list of all interviews
        /// </summary>
        /// <returns>The list of all interviews</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Interview>))]
        public IHttpActionResult GetAllInterviews()
        {
            var interviews = this.InterviewService.GetAllInterviews();

            if (interviews.Count() > 0)
            {
                return Ok(interviews);
            }

            return NotFound();
        }

        /// <summary>
        /// Get an interview by id
        /// </summary>
        /// <param name="id">Id of an interview to be fetched</param>
        /// <returns>An interview with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult GetInterview(int id)
        {
            if (id > 0)
            {
                var interview = this.InterviewService.GetOneInterviewById(id);

                if (interview != null)
                {
                    return Ok(interview);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new interview
        /// </summary>
        /// <param name="interview">An interview to be created</param>
        /// <returns>The Uri of newly created interview</returns>
        [HttpPost]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult PostInterview(Interview interview)
        {
            if (ModelState.IsValid)
            {
                this.InterviewService.CreateInterview(interview);

                try
                {
                    this.InterviewService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = interview.InterviewId }, interview);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing interview
        /// </summary>
        /// <param name="interview">Interview to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult PutInterview(Interview interview)
        {
            if (ModelState.IsValid)
            {
                this.InterviewService.UpdateInterview(interview);

                try
                {
                    this.InterviewService.SaveChanges();

                    return Ok(interview);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    if (!this.InterviewExists(interview.InterviewId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete existing interview
        /// </summary>
        /// <param name="id">Id of the interview to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult DeleteInterview(int id)
        {
            if (id > 0)
            {
                var interview = this.InterviewService.GetOneInterviewById(id);

                if (interview != null)
                {
                    this.InterviewService.DeleteInterview(interview);

                    try
                    {
                        this.InterviewService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.InterviewExists(interview.InterviewId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(interview);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// InterviewExists is used to check whether the interview is present in data context
        /// </summary>
        /// <param name="id">Id of the interview to check against</param>
        /// <returns>True if interview is present in data context, false otherwise</returns>
        private bool InterviewExists(int id)
        {
            return this.InterviewService.GetAllInterviews().Count(i => i.InterviewId == id) > 0;
        }
    }
}