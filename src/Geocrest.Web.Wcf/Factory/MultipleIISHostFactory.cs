namespace Geocrest.Web.Wcf.Factory
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    /// <summary>
    /// Represents a ServiceHostFactory for services hosted in an IIS 6 site configured with multiple identities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// IIS Web sites can be configured to use multiple identities such as <b>myserver</b> and 
    /// <b>myserver.com</b>. These mutliple identities will cause an exception in the standard 
    /// WCF <see cref="T:System.ServiceModel.Activation.ServiceHostFactory">ServiceHostFactory</see> appearing 
    /// as follows:
    /// </para><para>
    /// <em>This collection already contains an address with scheme http. There can be at most one address 
    /// per scheme in this collection.</em>
    /// </para><para>
    /// This class overcomes this problem by ensuring that only one identity is provided to the 
    /// <see cref="T:System.ServiceModel.ServiceHost">ServiceHost</see> constructor. This identity must be
    /// found in the configuration key named <c>validHosts</c>. This key is parsed as an array to enable 
    /// migration to different hosts without modification. However only one identity will be valid on each 
    /// IIS Web server.
    /// </para><para>
    /// This class has not been tested on IIS7/WAS hosts. There are new options in .NET 3.5 that should allow
    /// this work-around to be retired.
    /// </para><para>
    /// Several alternatives to this approach were tested. One of the more promising options is described 
    /// <see href='http://blog.ranamauro.com/2008/07/hosting-wcf-service-on-iis-site-with_25.html' target="_blank">here</see>.
    /// This approach places an empty URI array in the ServiceHost and then relies on absolute addresses 
    /// provided in the service endpoint configuration. 
    /// </para>
    /// </remarks>
    public class MultipleIISHostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Specifies characters that may be used to delimit IIS identities in the <c>validHosts</c> configuration key. 
        /// </summary>
        private char[] delimiters = new char[] { ';', ',', ' ' };

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost">ServiceHost</see> using one of the site
        /// addresses found in the input <paramref name="baseAddresses" /> if the name matches one of the
        /// names found in the <c>validHosts</c> configuration key.
        /// </summary>
        /// <param name="serviceType">The service to be hosted.</param>
        /// <param name="baseAddresses">An array of base addresses. These correspond to the multiple identities configured in IIS.</param>
        /// <returns>
        /// A ServiceHost from which the service will be hosted.
        /// </returns>
        /// is thrown if the <c>validHosts</c> configuration key is not found, empty, or if it does not contain a
        /// value that matches one of the detected IIS identities.
        /// <exception cref="T:System.ArgumentNullException">baseAddresses</exception>
        /// <exception cref="MultipleIISHostException">
        /// The configuration for this service is missing a 'validHosts' key. This key is required to filter out multiple identities that may be assigned to the IIS Web server.
        /// </exception>
        /// <exception cref="T:Geocrest.Web.Wcf.MultipleIISHostException">MultipleIISHostException</exception>
        protected override ServiceHost CreateServiceHost(System.Type serviceType, System.Uri[] baseAddresses)
        {
            if (baseAddresses == null) throw new ArgumentNullException("baseAddresses");
            string validHosts = ConfigurationManager.AppSettings["validHosts"];
            ////string validHosts = "bogusSiteIdentity1,bogusSiteIdentity2,bogusSiteIdentity3";
            ////string validHosts = "";

            if (string.IsNullOrEmpty(validHosts))
            {
                throw new MultipleIISHostException("The configuration for this service is missing a 'validHosts' key. This key is required to filter out multiple identities that may be assigned to the IIS Web server.");
            }

            string[] validHostArray = validHosts.Split(this.delimiters);

            for (int i = 0; i < baseAddresses.Length; i++)
            {
                for (int j = 0; j < validHostArray.Length; j++)
                {
                    if (string.Equals(baseAddresses[i].Host, validHostArray[j], StringComparison.OrdinalIgnoreCase))
                    {
                        return new ServiceHost(serviceType, baseAddresses[i]);
                        ////throw new System.Configuration.ConfigurationErrorsException("validHost found! " + baseAddresses[i].Host + " : "  + validHostArray[j]);
                    }
                }
            }

            // Build descriptive error information.
            string siteIdentities = string.Empty;
            for (int k = 0; k < baseAddresses.Length; k++)
            {
                siteIdentities += baseAddresses[k].Host + "\r\n";
            }

            string validHostsFormatted = string.Empty;
            for (int l = 0; l < validHostArray.Length; l++)
            {
                validHostsFormatted += validHostArray[l] + "\r\n";
            }

            throw new MultipleIISHostException(string.Format(new System.Globalization.CultureInfo("en-US"),
                "None of the base addresses detected from IIS are found in the 'validHosts' configuration key. Verify that at least one of the hosts found in the IIS settings for 'Multiple Web Site Identities' is included in this key. \r\n\r\n The formatted value of this key is: \r\n {0} \r\n Detected IIS site identities include: \r\n {1}", validHostsFormatted, siteIdentities));
        }
    }
}
