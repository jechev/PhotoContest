﻿namespace PhotoContest.Web.Controllers
{
    using Microsoft.AspNet.Identity;
    using System.Web.Mvc;
    using System.Linq;
    using PhotoContest.Data.Contracts;
    using Models.ViewModels;

    [Authorize]
    public class NotificationsController : BaseController
    {

        public NotificationsController(IPhotoContestData ctx) : base(ctx)
        {
        }

        //[HttpGet]
        [Route(Name = "NotificationsRoute")]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnReadNotifications()
        {
            var userID = User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userID);

            var pagedNotifications = user.Notifications.Where(n => n.IsRead == true)
                .AsQueryable()
                .Select(PagedNotificationViewModel.Create)
                .OrderByDescending(n => n.SendOn).ToList();

            return PartialView("_PagedNotifications", pagedNotifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnNewNotifications()
        {
            var userID = User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userID);

            var pagedNotifications = user.Notifications.Where(n => n.IsRead == false)
                .AsQueryable()
                .Select(PagedNotificationViewModel.Create)
                .OrderByDescending(n => n.SendOn).ToList();

            return PartialView("_PagedNotifications", pagedNotifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Read(int id)
        {
            var notification = this.Data.Notifications.GetById(id);
            notification.IsRead = true;
            this.Data.SaveChangesAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int CountNotifications()
        {
            var userId = User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);
            int num = user.Notifications.Count();

            return num;
        }
    }
}