﻿//------------------------------------------------------------------------------------------------------------------------------------
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
   public class NewExportJob //: INewExportJob
   {
      private const string LAST_VERSION = "1.5";
      private const string DEFAULT_FORMAT = "STEPAP242XML";

      public NewExportJob()
      {
         Version = LAST_VERSION;
         Format = DEFAULT_FORMAT;
         SelectorMode = null;
      }

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

      [JsonPropertyName("selectorMode")]
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public bool? SelectorMode { get; set; }
   }
}
