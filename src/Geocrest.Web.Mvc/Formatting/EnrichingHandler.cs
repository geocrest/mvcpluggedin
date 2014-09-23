namespace Geocrest.Web.Mvc.Formatting
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Allows the <see cref="T:System.Net.Http.HttpResponseMessage"/> to be enriched by any 
    /// <see cref="T:Geocrest.Web.Mvc.Formatting.IResponseEnricher"/> found in
    /// the global configuration.
    /// </summary>
    internal class EnrichingHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Threading.Tasks.Task`1" /> containing <see cref="T:System.Net.Http.HttpResponseMessage" />.
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <remarks>This override allows interception of the response in order to add additional outgoing information.</remarks>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    var enrichers = request.GetConfiguration().GetResponseEnrichers();

                    return enrichers.Where(e => e.CanEnrich(response))
                        .Aggregate(response, (resp, enricher) => enricher.Enrich(response));
                });
        }
    }
}
