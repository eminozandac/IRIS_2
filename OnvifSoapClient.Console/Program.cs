using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CameraControl.SoapClient.Models.Headers;

namespace OnvifSoapClient.Console
{
    internal class Program
    {
        private static void Main()
        {
            var usernameToken = KnownHeader.Oasis.Security.UsernameTokenWithPasswordDigest("admin", "admin", DateTime.Now).UsernameToken;

            var profilesRequest = GetProfiles(usernameToken.Username, usernameToken.Password.Value, usernameToken.Nonce.Value, usernameToken.Created);
            var result = ExecuteRawAsync(profilesRequest, "http://www.onvif.org/ver10/media/wsdl/GetProfiles").Result;

            System.Console.WriteLine(result);
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }

        private static async Task<string> ExecuteRawAsync(string request, string action)
        {
            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://96.69.46.121:580/onvif/device_service")
                {
                    Content = new StringContent(request, Encoding.UTF8)
                };
                requestMessage.Content.Headers.Clear();

                var contentTypeValue = $"application/soap+xml;charset=UTF-8;action=\"{action}\"";
                if (!requestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", contentTypeValue))
                {
                    throw new Exception($"Could not assign '{contentTypeValue}' as the 'Content-Type' header value");
                }

                var response = await httpClient.SendAsync(requestMessage);
                var responseText = await response.Content.ReadAsStringAsync();

                return responseText;
            }
        }

        private static string GetProfiles(string username, string passowordHash, string nonceHash, string created)
        {
            return 
"<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\">" +
    "<s:Header>" +
        "<Security s:mustUnderstand=\"1\" xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
            "<UsernameToken>" +
                "<Username>" +
                    $"{username}" +
                "</Username>" +
                "<Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">" +
                    $"{passowordHash}" +
                "</Password>" +
                "<Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">" +
                    $"{nonceHash}" +
                "</Nonce>" +
                "<Created xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
                    $"{created}" +
                "</Created>" +
            "</UsernameToken>" +
        "</Security>" +
    "</s:Header>" +
    "<s:Body>" +
        "<GetProfiles xmlns=\"http://www.onvif.org/ver10/media/wsdl\" />" +
    "</s:Body>" +
"</s:Envelope>";
        }
    }
}
