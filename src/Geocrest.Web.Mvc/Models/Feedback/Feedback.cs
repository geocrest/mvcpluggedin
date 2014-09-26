namespace Geocrest.Web.Mvc.Models.Feedback
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents feedback from a user.
    /// </summary>
    public class Feedback : Resource
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
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>
        /// The publish date.
        /// </value>
        [Required]
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// Gets or sets the name of the submitter.
        /// </summary>
        /// <value>
        /// The user's name.
        /// </value>
        [Required(ErrorMessage="*")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the email of the submitter.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the feedback subject id.
        /// </summary>
        /// <value>
        /// The feedback subject id.
        /// </value>
        [Display(Name = "Subject")]
        public int FeedbackSubjectId { get; set; }
        /// <summary>
        /// Gets or sets the user's comment.
        /// </summary>
        /// <value>
        /// The user's comment.
        /// </value>
        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this comment is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this comment is public; otherwise, <c>false</c>.
        /// </value>
        bool IsPublic { get; set; }
        /// <summary>
        /// Gets or sets the feedback subject.
        /// </summary>
        /// <value>
        /// The feedback subject.
        /// </value>
        public virtual FeedbackSubject FeedbackSubject { get; set; }
    }
}
