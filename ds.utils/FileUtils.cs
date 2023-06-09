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

namespace ds.utils
{
   public class FileUtils
   {
      public async Task<(bool, string)> Download(string url, string downloadFolder, string defaultFilename)
      {
         string? __fullFilename = null;

         using (HttpClient downloadClient = new HttpClient(new HttpClientHandler()))
         using (HttpResponseMessage res = await downloadClient.GetAsync(url))
         {
            if (res.Content.Headers.ContentDisposition != null)
            {
               defaultFilename = res.Content.Headers.ContentDisposition.FileNameStar!;
            }

            if (!downloadFolder.EndsWith(Path.DirectorySeparatorChar))
            {
               downloadFolder += Path.DirectorySeparatorChar;
            }

            //TODO: CHECK validity of folder and filename
            __fullFilename = $"{downloadFolder}{defaultFilename}";

            using (var writer = File.OpenWrite(__fullFilename))
            using (Stream streamToReadFrom = await res.Content.ReadAsStreamAsync())
            {
               streamToReadFrom.CopyTo(writer);
            }
         }

         return (true, __fullFilename);
      }
   }
}