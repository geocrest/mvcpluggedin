namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
using System.Runtime.Serialization;

    /// <summary>
    /// Represents default XML parameter documentation that should be replaced by matching values from a 
    /// resources file.
    /// </summary>
    [Serializable]
    public class DefaultXmlReplacements : Dictionary<string,string>
    {
        private IDictionary<string, string> tokens = new Dictionary<string,string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.DefaultXmlReplacements" /> class.
        /// </summary>
        public DefaultXmlReplacements()
        {
            this.Add("includeObjects", "if set to true [include objects].");
            this.Add("outSR", "The out SR.");
            this.Add("inSR", "The in SR.");
            foreach (var kvp in ConfigurationManager.AppSettings)
            {

            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.DefaultXmlReplacements" /> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        protected DefaultXmlReplacements(SerializationInfo info, StreamingContext context) :base(info,context)
        {
            
        }
        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets a collection of tokens that should be replaced at run-time.
        /// </summary>
        /// <value>
        /// The tokens to replace.
        /// </value>
        public IDictionary<string,string> Tokens 
        { 
            get { return this.tokens; } 
        }
        //public string ReplaceToken(string stringToReplace, KeyValuePair<string, string> token)
        //{
        //    if (string.IsNullOrWhiteSpace(stringToReplace)) return stringToReplace;
        //    Throw.IfArgumentNull(token, "token");
        //    if (!stringToReplace.Contains(token.Key)) return stringToReplace;
        //    string returnvalue = string.Empty;
        //   // if (ConfigurationManager.
        //}
    }
}
