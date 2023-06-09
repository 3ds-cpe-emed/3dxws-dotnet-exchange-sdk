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

using System.Text.Json.Serialization;

namespace ds.enovia.exchange.model
{
   public class ExportJob //: IExportJob
   {
      [JsonPropertyName("retried")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public bool? Retried { get; set; }

      [JsonPropertyName("retryIdentifier")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public long? RetryIdentifier { get; set; }

      [JsonPropertyName("creationDate")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string CreationDate { get; set; } = null!;

      [JsonPropertyName("submissionDate")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string SubmissionDate { get; set; } = null!;

      [JsonPropertyName("startDate")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string StartDate { get; set; } = null!;

      [JsonPropertyName("endDate")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string EndDate { get; set; } = null!;

      [JsonPropertyName("status")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string Status { get; set; } = null!;

      [JsonPropertyName("creator")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string Creator { get; set; } = null!;

      [JsonPropertyName("timeToKeep")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string TimeToKeep { get; set; } = null!;

      [JsonPropertyName("timeToLive")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string TimeToLive { get; set; } = null!;

      [JsonPropertyName("progress")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string Progress { get; set; } = null!;

      [JsonPropertyName("credits")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public long? Credits { get; set; }

      [JsonPropertyName("consumableObjects")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public long? ConsumableObjects { get; set; }

      [JsonPropertyName("nbTotalObjects")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public long? TotalNumberOfObjects { get; set; }

      [JsonPropertyName("nbTotalResults")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public long? TotalNumberOfResults { get; set; }

      [JsonPropertyName("resulturls")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public List<string> ResultUrls { get; set; } = null!;

      [JsonPropertyName("nbTotalReports")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public long? TotalNumberOfReports { get; set; }

      [JsonPropertyName("reporturls")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public List<string> ReportUrls { get; set; } = null!;

      [JsonPropertyName("version")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string Version { get; set; } = null!;

      [JsonPropertyName("title")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string Title { get; set; } = null!;

      [JsonPropertyName("format")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public string Format { get; set; } = null!;

      [JsonPropertyName("objects")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public List<object> Objects { get; set; } = null!;

      [JsonPropertyName("parameters")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public List<object> Parameters { get; set; } = null!;

      [JsonPropertyName("hasCompleted")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
      public bool HasCompleted { get { return !string.IsNullOrEmpty(EndDate); } }

      [JsonPropertyName("hasSucceeded")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
      public bool HasSucceeded { get { return Status?.ToLowerInvariant().Equals("succeeded") == true; } }
   }
}
