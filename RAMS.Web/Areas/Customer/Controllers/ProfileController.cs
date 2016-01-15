﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;

namespace RAMS.Web.Areas.Customer.Controllers
{
    /// <summary>
    /// ProfileController implements employee profile related methods
    /// </summary>
    public class ProfileController : BaseController
    {
        /// <summary>
        /// Default action method that returns main view of Profile controller
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <param name="message">Message that will be displayed in the view. This message will inform the user about success or failure of particular operation</param>
        /// <returns>Main view of Profile controller</returns>
        [HttpGet]
        public ActionResult Index(string message = "")
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity.HasClaim("UserType", "Agent"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Agency" });
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                return View("Index", null, message);
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        #region Profile Details
        /// <summary>
        /// ProfileDetails method gets employee's details and passes it to _ProfileDetails partial view
        /// </summary>
        /// <returns>_ProfileDetails partial view with current user's employee details</returns>
        [HttpGet]
        public async Task<PartialViewResult> ProfileDetails()
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", User.Identity.Name));

            if (response.IsSuccessStatusCode)
            {
                var clientProfileDetailsViewModel = Mapper.Map<Client, ClientProfileDetailsViewModel>(await response.Content.ReadAsAsync<Client>());

                return PartialView("_ProfileDetails", clientProfileDetailsViewModel);
            }

            return PartialView("_ProfileDetails");
        }

        /// <summary>
        /// UploadProfilePicture method gets current user's user name and passes it to _UploadProfilePicture partial view 
        /// </summary>
        /// <returns>_UploadProfilePicture partial view with current user's user name passed in to it in a AdminProfilePictureViewModel view model</returns>
        [HttpGet]
        public PartialViewResult UploadProfilePicture()
        {
            var clientProfilePictureViewModel = new ClientProfilePictureViewModel(User.Identity.Name);

            return PartialView("_UploadProfilePicture", clientProfilePictureViewModel);
        }

        /// <summary>
        /// UploadProfilePicture method determines what is the content type of the uploaded file and if file content type is not supported passes an error message to Index action method. Otherwise, saves the file.
        /// </summary>
        /// <param name="model">View model with information required to save the file</param>
        /// <returns>Redirects to Index view with success message, or failure message</returns>
        [HttpPost]
        public ActionResult UploadProfilePicture(ClientProfilePictureViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (model.ProfilePicture.ContentType == "image/jpeg") // If file is .jpg
                    {
                        model.ProfilePicture.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")); // Save file

                        if(System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")); // Delete other files if present
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")); // Delete other files if present
                        }

                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully updated." });
                    }
                    else if (model.ProfilePicture.ContentType == "image/gif") // If file is .gif
                    {
                        model.ProfilePicture.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")); // Save file

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")); // Delete other files if present
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")); // Delete other files if present
                        }

                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully updated." });
                    }
                    else if (model.ProfilePicture.ContentType == "image/png") // If file is .png
                    {
                        model.ProfilePicture.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")); // Save file

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")); // Delete other files if present
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")); // Delete other files if present
                        }

                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully updated." });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Unsupported image type. Please try again using supported image type." });
                    }
                }
                catch (UnauthorizedAccessException ex) // IMPORTANT - This exception will probably be thrown on the server, unless we change ProfilePictures folder's permissions right away
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "File could NOT be saved due to lack of permissions. Please review exception log for more details." });
                }
                catch(System.IO.IOException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "An exception has occured. Please review exception log for more details." });
                }

            }

            return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "An error occured while attmpting to upload a profile picture. Profile picture has NOT been updated." });
        }

        /// <summary>
        /// DeleteProfilePicture method deletes profile picture for current user
        /// </summary>
        /// <returns>Redirects to Index view with success message, or failure message</returns>
        [HttpGet]
        public ActionResult DeleteProfilePicture()
        {
            var success = false;

            try
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), User.Identity.Name + ".jpg")))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), User.Identity.Name + ".jpg")); // Delete .jpg file if it exists

                    success = true;
                }

                if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), User.Identity.Name + ".png")))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), User.Identity.Name + ".png")); // Delete .png file if it exists

                    success = true;
                }

                if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), User.Identity.Name + ".gif")))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), User.Identity.Name + ".gif")); // Delete .gif file if it exists

                    success = true;
                }

                if (success)
                {
                    return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully deleted." });
                }

                return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture could NOT be deleted at this time." });
            }
            catch (System.IO.IOException ex)
            {
                // Log exception
                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "An exception has occured. Please review exception log for more details." });
            }
        }
        #endregion

        /// <summary>
        /// GetNotificationList method gets the list of notifications for current user and passes it to _NotificationList partial view
        /// </summary>
        /// <returns>_NotificationList partial view with the list of notifications for current user (If any)</returns>
        public async Task<PartialViewResult> GetNotificationList()
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("Notification?clientUserName={0}", User.Identity.Name));

            if (response.IsSuccessStatusCode)
            {
                var notificationListViewModel = Mapper.Map<List<Notification>, List<NotificationListViewModel>>(await response.Content.ReadAsAsync<List<Notification>>());

                return PartialView("_NotificationList", notificationListViewModel);
            }

            return PartialView("_NotificationList");
        }
    }
}