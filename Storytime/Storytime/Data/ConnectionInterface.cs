using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Storytime.Data
{
    public class ConnectionInterface
    {
        private HttpClient client;

        public ConnectionInterface()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        //Insert statement
        public async Task Upload(uint age, string file)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[Storytime / HTTP] " + file);

                HttpContent ageContent = new StringContent(age.ToString());
                HttpContent timeContent = new StringContent(File.GetCreationTime(file).ToString("yyyy-MM-dd HH:mm:ss"));
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    HttpContent fileStreamContent = new StreamContent(fs);

                    TimeSpan t = File.GetCreationTime(file) - new DateTime(1970, 1, 1);
                    long timestamp = (long)t.TotalSeconds;

                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(fileStreamContent, "fileToUpload", file);
                        formData.Add(ageContent, "age");
                        formData.Add(timeContent, "rtime");

                        System.Diagnostics.Debug.WriteLine("[Storytime / HTTP] " + "Fine so far");

                        var response = await client.PostAsync("https://liacs.leidenuniv.nl/~s1680129/kv/upload.php", formData);

                        System.Diagnostics.Debug.WriteLine("[Storytime / HTTP] " + "Fine so far 2");

                        var responseString = await response.Content.ReadAsStringAsync();

                        System.Diagnostics.Debug.WriteLine("[Storytime / HTTP] " + responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Storytime / HTTP] " + ex.ToString());
            }
        }
    }
}
