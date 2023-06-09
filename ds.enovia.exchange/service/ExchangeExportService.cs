//------------------------------------------------------------------------------------------------------------------------------------
// Copyright 2023 Dassault Systèmes - CPE EMED
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//------------------------------------------------------------------------------------------------------------------------------------

using ds.authentication;
using ds.enovia.exchange.exception;
using ds.enovia.exchange.model;
using ds.enovia.service;
using System.Net.Http.Json;
using System.Text.Json;

namespace ds.enovia.exchange.service
{
   public class ExchangeExportService : EnoviaBaseService
   {
      private const string BASE_RESOURCE = "/resources/v1/modeler/exchangejobs/exports";
      private const string LOGIN_CHECK = "/resources/v1/modeler/logincheck";

      private const string X_REQUESTED_WITH_HEADER = "X-Requested-With";
      private const string X_REQUESTED_WITH_HEADER_VALUE = "batch";

      public string GetBaseResource()
      {
         return BASE_RESOURCE;
      }

      public ExchangeExportService(string enoviaService, IPassportAuthentication _passport) : base(enoviaService, _passport)
      {
      }

      public async Task<bool> Initialize()
      {
         string resourceURI = LOGIN_CHECK;

         HttpResponseMessage requestResponse = await GetAsync(resourceURI);

         if (requestResponse.StatusCode != System.Net.HttpStatusCode.OK)
         {
            //handle according to established exception policy
            throw (new LoginCheckException(requestResponse));
         }

         return true;
      }

      public async Task<ExportJobRef?> CreateExportJob(NewExportJob _exportJob)
      {
         if (_exportJob is null)
         {
            throw new ArgumentNullException(nameof(_exportJob));
         }

         string resourceURI = GetBaseResource();

         // headers
         Dictionary<string, string> headerParams = new Dictionary<string, string>{
            { X_REQUESTED_WITH_HEADER, X_REQUESTED_WITH_HEADER_VALUE }
         };

         string exportJobPayload = JsonSerializer.Serialize(_exportJob);
         HttpResponseMessage requestResponse = await PostAsync(resourceURI, _body: exportJobPayload, _headers: headerParams, _requiresCsrfToken: false);

         if (requestResponse.StatusCode != System.Net.HttpStatusCode.Created)
         {
            //handle according to established exception policy
            throw (new CreateExportJobException(requestResponse));
         }

         return await requestResponse.Content.ReadFromJsonAsync<ExportJobRef>();
      }

      public async Task<bool> SubmitExportJob(string _exportJobId)
      {
         if (_exportJobId is null)
         {
            throw new ArgumentNullException(nameof(_exportJobId));
         }

         string resourceURI = $"{GetBaseResource()}/{_exportJobId}/submit";

         // headers
         Dictionary<string, string> headerParams = new Dictionary<string, string> {
            { X_REQUESTED_WITH_HEADER, X_REQUESTED_WITH_HEADER_VALUE }
         };

         HttpResponseMessage requestResponse = await PostAsync(resourceURI, _headers: headerParams, _requiresCsrfToken: false);

         if (requestResponse.StatusCode != System.Net.HttpStatusCode.OK)
         {
            //handle according to established exception policy
            throw (new SubmitExportJobException(requestResponse));
         }

         return true;
      }

      public async Task<ExportJob?> GetExportJob(string _exportJobId)
      {
         if (_exportJobId is null)
         {
            throw new ArgumentNullException(nameof(_exportJobId));
         }

         string resourceURI = $"{GetBaseResource()}/{_exportJobId}";

         // headers
         Dictionary<string, string> headerParams = new Dictionary<string, string> {
            { X_REQUESTED_WITH_HEADER, X_REQUESTED_WITH_HEADER_VALUE }
         };

         HttpResponseMessage requestResponse = await GetAsync(resourceURI, _headers: headerParams, _requiresCsrfToken: false);

         if (requestResponse.StatusCode != System.Net.HttpStatusCode.OK)
         {
            //handle according to established exception policy
            throw (new GetExportJobException(requestResponse));
         }

         return await requestResponse.Content.ReadFromJsonAsync<ExportJob>(); ;
      }
   }
}
