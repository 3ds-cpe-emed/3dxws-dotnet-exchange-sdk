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
using ds.authentication.redirection;
using ds.enovia.exchange.model;
using ds.enovia.exchange.service;
using ds.enovia.model;
using ds.utils;
using ws3dx.shared.data.impl;

namespace ds.enovia.exchange.tests
{
   public class ExchangeExportServiceTests
   {
      private const string DS3DXWS_AUTH_USERNAME = "DS3DXWS_AUTH_USERNAME";
      private const string DS3DXWS_AUTH_PASSWORD = "DS3DXWS_AUTH_PASSWORD";
      private const string DS3DXWS_AUTH_PASSPORT = "DS3DXWS_AUTH_PASSPORT";
      private const string DS3DXWS_AUTH_ENOVIA = "DS3DXWS_AUTH_ENOVIA";
      private const string DS3DXWS_AUTH_TENANT = "DS3DXWS_AUTH_TENANT";
      private const string DS3DXWS_EXCHANGE_URL = "DS3DXWS_EXCHANGE_URL";

      private string? m_username = string.Empty;
      private string? m_password = string.Empty;
      private string? m_passportUrl = string.Empty;
      private string? m_serviceUrl = string.Empty;
      private string? m_tenant = string.Empty;
      private string? m_exchangeUrl = string.Empty;

      private UserInfo m_userInfo = null!;

      private const string PART_NUMBER_TEST = "AAA27:0000002";

      public async Task<IPassportAuthentication> Authenticate()
      {
         UserPassport passport = new UserPassport(m_passportUrl);

         UserInfoRedirection userInfoRedirection = new UserInfoRedirection(m_serviceUrl, m_tenant)
         {
            Current = true,
            IncludeCollaborativeSpaces = true,
            IncludePreferredCredentials = true
         };

         m_userInfo = await passport.CASLoginWithRedirection<UserInfo>(m_username, m_password, false, userInfoRedirection);

         Assert.IsNotNull(m_userInfo);

         StringAssert.AreEqualIgnoringCase(m_userInfo.name, m_username);

         Assert.IsTrue(passport.IsCookieAuthenticated);

         return passport;
      }

      [SetUp]
      public void Setup()
      {
         m_username = Environment.GetEnvironmentVariable(DS3DXWS_AUTH_USERNAME, EnvironmentVariableTarget.User); // e.g. AAA27
         m_password = Environment.GetEnvironmentVariable(DS3DXWS_AUTH_PASSWORD, EnvironmentVariableTarget.User); // e.g. your password
         m_passportUrl = Environment.GetEnvironmentVariable(DS3DXWS_AUTH_PASSPORT, EnvironmentVariableTarget.User); // e.g. https://eu1-ds-iam.3dexperience.3ds.com:443/3DPassport

         m_serviceUrl = Environment.GetEnvironmentVariable(DS3DXWS_AUTH_ENOVIA, EnvironmentVariableTarget.User); // e.g. https://r1132100982379-eu1-space.3dexperience.3ds.com:443/enovia
         m_tenant = Environment.GetEnvironmentVariable(DS3DXWS_AUTH_TENANT, EnvironmentVariableTarget.User); // e.g. R1132100982379

         m_exchangeUrl = Environment.GetEnvironmentVariable(DS3DXWS_EXCHANGE_URL, EnvironmentVariableTarget.User); //
      }

      public string GetDefaultSecurityContext()
      {
         return m_userInfo.preferredcredentials.ToString();
      }

      public ExchangeExportService ServiceFactoryCreate(IPassportAuthentication _passport, string _serviceUrl, string _tenant)
      {
         ExchangeExportService __engItemService = new ExchangeExportService(_serviceUrl, _passport);
         __engItemService.Tenant = _tenant;
         __engItemService.SecurityContext = GetDefaultSecurityContext();
         return __engItemService;
      }

      [TestCase("A447C3205D1D4DC19A61E99B41E884C7")]
      public async Task CreateExportJob(string rootEngItemId)
      {
         ExchangeExportService service = ServiceFactoryCreate(await Authenticate(), m_exchangeUrl!, m_tenant!);

         await service.Initialize();

         TypedUriIdentifier id = new TypedUriIdentifier();
         id.Type = "VPMReference";
         id.Source = m_serviceUrl;
         id.RelativePath = $"/resource/v1/dseng/VPMReference/{rootEngItemId}";
         id.Identifier = rootEngItemId;

         EngineeringDomainExportParameter engineeringDomainExportParam = new EngineeringDomainExportParameter(); ;
         engineeringDomainExportParam.ExpandDepth = "0";

         NewExportJob newExportJob = new NewExportJob();
         newExportJob.Title = "ERP Export test";

         newExportJob.Objects = new List<object> {
            id
         };

         newExportJob.Parameters = new List<object> {
            engineeringDomainExportParam
         };

         ExportJobRef? newJobRef = await service.CreateExportJob(newExportJob);

         int pollingIntervalSecs = 20;
         int timeoutSecs = 5 * 60;
         bool timeOutRaised = false;

         if (!await service.SubmitExportJob(newJobRef.Identifier))
         {
            return;
         }

         DateTime pollingStart = DateTime.Now;

         ExportJob exportJob = null;

         do
         {

            if (DateTime.Now > pollingStart.AddSeconds(timeoutSecs))
            {
               timeOutRaised = true;
               break;
            }

            Thread.Sleep(pollingIntervalSecs * 1000);

            exportJob = await service.GetExportJob(newJobRef.Identifier);

         }
         while ((exportJob != null) && (!exportJob.HasCompleted));

         if (timeOutRaised)
         {
            //TODO:
         }

         if (exportJob == null)
         {
            throw new Exception();
         }

         if (!exportJob.HasSucceeded)
         {
            throw new Exception();
         }

         FileUtils fileUtils = new FileUtils();

         await fileUtils.Download(exportJob.ResultUrls.First(), "c:/temp", "results.out");

         await fileUtils.Download(exportJob.ReportUrls.First(), "c:/temp", "report.out");

      }
   }
}