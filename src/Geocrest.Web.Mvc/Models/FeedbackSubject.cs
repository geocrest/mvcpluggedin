namespace Geocrest.Web.Mvc.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the subject matter of feedback.
    /// </summary>
    public class FeedbackSubject
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the feedback subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [Required]
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the feedback comments within this subject.
        /// </summary>
        /// <value>
        /// The feedback comments.
        /// </value>
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
