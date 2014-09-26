namespace Geocrest.Web.Mvc.Controllers
{
    using Geocrest.Data.Contracts;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Infrastructure.Mail;
    using Geocrest.Web.Mvc.Models.Account;
    using Geocrest.Web.Mvc.Models.Feedback;
    using Geocrest.Web.Mvc.Resources;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using WebMatrix.WebData;

    /// <summary>
    /// Provides actions for creating and listing user feedback.
    /// </summary>
    public class FeedbackController : BaseController
    {
        private readonly IRepository repository;
        private readonly IMailSender mailer;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.FeedbackController" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mailer">The mail service used to send email.</param>
        public FeedbackController(IRepository repository, IMailSender mailer)
        {
            Throw.IfArgumentNull(repository, "repository");
            Throw.IfArgumentNull(mailer, "mailer");
            this.repository = repository;
            this.mailer = mailer;
        }
        /// <summary>
        /// Returns an index page for displaying feedback.
        /// </summary>
        /// <returns></returns>
        [AdminOnly]
        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns a list of all feedback.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult List()
        {
            ViewBag.FeedbackSubjects = repository.All<FeedbackSubject>();
            var fb = this.repository.AllIncluding<Feedback>(x => x.FeedbackSubject).OrderByDescending(x => x.PublishDate);
            return PartialView(fb);
        }
        /// <summary>
        /// Returns a list of testimonial feedback.
        /// </summary>
        /// <param name="n">The number of records to return. The default is 5.</param>
        /// <returns></returns>
        public virtual ActionResult Testimonials(int n = 5)
        {
            var fb = this.repository.AllIncluding<Feedback>(x => x.FeedbackSubject)
                .Where(x => x.FeedbackSubject.Subject == "Testimonial")
                .OrderByDescending(x => x.PublishDate)
                .Take(n);
            return PartialView(fb);
        }
        /// <summary>
        /// Returns a form for adding new feedback.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            ViewBag.FeedbackSubjects = repository.All<FeedbackSubject>();
            return PartialView();
        }
        /// <summary>
        /// Inserts a new feedback comment.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(Feedback model)
        {
            ViewBag.FeedbackSubjects = repository.All<FeedbackSubject>();
            if (ModelState.IsValid)
            {
                try
                {
                    model.PublishDate = DateTime.UtcNow;
                    this.repository.Insert(model);
                    this.repository.Save();

                    emailResult result;
                    var admins = Roles.GetUsersInRole(BaseApplication.AdminRole);
                    foreach (var admin in admins)
                    {
                        BaseProfile profile;
                        if (Membership.Provider is SimpleMembershipProvider)
                        {
                            profile = BaseProfile.CreateProfile(this.repository, admin);
                        }
                        else
                        {
                            profile = BaseProfile.CreateProfile(admin);
                        }
                        if (profile != null)
                        {
                            var subject = this.repository.Find<FeedbackSubject>(model.FeedbackSubjectId).Subject;
                            mailer.Send(profile.Email, string.Format("{0} feedback submitted", profile.Application),
                            string.Format(@"<h3>A new comment with the category <em>{0}</em> has been posted</h3>
<strong>Submitted by</strong>: {1}<br /><strong>Reply to</strong>: {2}<p>{3}</p>", subject,
                            model.Name, model.Email, model.Comment), out result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return HttpStatusCode(System.Net.HttpStatusCode.InternalServerError, ex);
                }
                return Json(new
                {
                    success = true,
                    message = FormMessages.FeedbackPostSuccess,
                    content = this.RenderPartialViewToString("list",
                    this.repository.AllIncluding<Feedback>(x => x.FeedbackSubject).OrderByDescending(x => x.PublishDate))
                });
            }
            else
            {
                return HttpStatusCode(System.Net.HttpStatusCode.BadRequest, new InvalidOperationException(
                    string.Join("; ", ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)))));
            }
        }
        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                this.repository.Delete<Feedback>(id);
                this.repository.Save();
                return Json(new
                {
                    success = true,
                    message = FormMessages.FeedbackDeleteSuccess,
                    id = id
                });
            }
            catch (Exception ex)
            {
                return HttpStatusCode(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
