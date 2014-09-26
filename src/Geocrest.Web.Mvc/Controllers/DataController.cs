namespace Geocrest.Web.Mvc.Controllers
{
    using Geocrest.Data.Contracts;
    using Ninject;
    using System.Linq;
    using System.Web.Mvc;
    /// <summary>
    /// Provides a controller with an <see cref="T:Geocrest.Data.Contracts.IRepository"/> property.
    /// </summary>
    public abstract class DataController<T> : BaseController where T : Resource
    {
        /// <summary>
        /// Gets the repository used to retrieve database entries.
        /// </summary>
        /// <value>
        /// The repository interface.
        /// </value>
        [Inject]
        public virtual IRepository Repository { get; set; }
        /// <summary>
        /// Returns the main view for this controller.
        /// </summary>
        public virtual ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Returns a partial view containing a list of model items for this controller.
        /// </summary>
        public virtual ActionResult List()
        {
            return PartialView(this.Repository.All<T>());
        }
        /// <summary>
        /// Returns a view for updating the model with the specified id.
        /// </summary>
        /// <param name="id">The id of the model to update.</param>
        /// <returns></returns>
        public virtual ActionResult Edit(string id)
        {
            T model = this.Repository.Find<T>(id);
            return PartialView(model);
        }
        /// <summary>
        /// Returns a view for creating a new model.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            return PartialView("_createoredit");
        }
        /// <summary>
        /// Posts a new model into permanent storage.
        /// </summary>
        /// <param name="model">The model to save.</param>
        /// <returns>
        /// A JSON object containing the following properties:
        /// <list type="bullet">
        /// <item><c>success</c> (boolean): indicates if the operation was successful.</item>
        /// <item><c>message</c> (string): a status message.</item>
        /// <item><c>content</c> (string): HTML content used to update the rendered view.</item>
        /// </list>
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(T model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.Repository.Insert<T>(model);
                    this.Repository.Save();
                    return Json(new
                    {
                        success = true,
                        message = string.Format(Geocrest.Web.Mvc.Resources.FormMessages.PostSuccess, "model"),
                        content = this.RenderPartialViewToString("list", this.Repository.All<T>().ToList())
                    });
                }
                catch (System.Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = string.Format(Geocrest.Web.Mvc.Resources.FormMessages.PostFailure, "model", ex.Message)
                    });
                }
            }
            return Json(new
            {
                success = false,
                message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)))
            });
        }

        /// <summary>
        /// Posts an updated model into permanent storage.
        /// </summary>
        /// <param name="model">The model being updated.</param>
        /// <returns>
        /// A JSON object containing the following properties:
        /// <list type="bullet">
        /// <item><c>success</c> (boolean): indicates if the operation was successful.</item>
        /// <item><c>message</c> (string): a status message.</item>
        /// <item><c>content</c> (string): HTML content used to update the rendered view.</item>
        /// </list>
        /// </returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(T model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.Repository.Update<T>(model);
                    this.Repository.Save();
                    return Json(new
                    {
                        success = true,
                        message = string.Format(Geocrest.Web.Mvc.Resources.FormMessages.PutSuccess, "model"),
                        content = this.RenderPartialViewToString("list", this.Repository.All<T>().ToList())
                    });
                }
                catch (System.Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = string.Format(Geocrest.Web.Mvc.Resources.FormMessages.PutFailure, "model", ex.Message)
                    });
                }
            }
            return Json(new
            {
                success = false,
                message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)))
            });
        }

        /// <summary>
        /// Deletes the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id of the model.</param>
        /// <returns>
        /// A JSON object containing the following properties:
        /// <list type="bullet">
        /// <item><c>success</c> (boolean): indicates if the operation was successful.</item>
        /// <item><c>message</c> (string): a status message.</item>
        /// <item><c>id</c> (string): the id of the deleted item.</item>
        /// </list>
        /// </returns>
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(string id)
        {
            try
            {
                this.Repository.Delete<T>(id);
                this.Repository.Save();
                return Json(new
                {
                    success = true,
                    message = string.Format(Geocrest.Web.Mvc.Resources.FormMessages.DeleteSuccess, "model"),
                    id = id
                });
            }
            catch (System.Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = string.Format(Geocrest.Web.Mvc.Resources.FormMessages.DeleteFailure, "model", ex.Message)
                });
            }
        }
            }
}
