﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoContest.Data;
using PhotoContest.Data.Contracts;

namespace PhotoContest.Web.Controllers
{
    [Authorize]
    public class VoteController : BaseController
    {

        public VoteController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult AddVote(int id)
        {
            var picture = this.Data.Images.GetById(id);
            if (picture == null)
            {
                return this.HttpNotFound("Image is not found.");
            }
            var loginUser = User.Identity.GetUserId();
            var currentUser = this.Data.Users.GetById(loginUser);
            currentUser.VotedPictures.Add(picture);
            this.Data.SaveChanges();
            var count = picture.Votes.Count;
            return this.Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}