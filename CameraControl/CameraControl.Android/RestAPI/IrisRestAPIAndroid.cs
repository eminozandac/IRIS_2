using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CameraControl.Portable.RestAPI;
using Newtonsoft.Json;

namespace CameraControl.Droid
{
	public class IrisRestAPIAndroid : IIrisRestAPI
	{
		private const int MAXBUFFER = 1024 * 64;
		byte[] data;

		public X509Certificate2 Certificate { get; set; }

		public string Token { get; set; }
		private string username;
		private string password;
		public const string DateFormatString = "MM/dd/yyyy HH:mm:ss";
		private static JsonSerializerSettings jss = new JsonSerializerSettings();
		public String UrlAPI { get; set; } = "https://localhost:5003";
		private static IrisRestAPIAndroid _IrisRestAPI = null;

		/// <summary>
		/// Time Out
		/// </summary>
		public TimeSpan Timeout { get; set; }

		public void InitCertificate()
		{
			var assetManger = Android.App.Application.Context;
			using (var fileFromAsset = assetManger.Assets.Open("IrisCovertMobile.pfx"))
			{
				using (var memstream = new MemoryStream())
				{
					fileFromAsset.CopyTo(memstream);
					byte[] dataCert = memstream.ToArray();

					Certificate = new X509Certificate2(dataCert, "123456");
				}
			}
		}


		private static JsonSerializerSettings Jss()
		{
			return jss;
		}

		internal IrisRestAPIAndroid()
		{
			Timeout = new TimeSpan(0, 0, 60);
			jss.DateFormatString = "MM/dd/yyyy HH:mm:ss";
		}

		static public IrisRestAPIAndroid Instance
		{
			get
			{
				if (_IrisRestAPI == null)
				{
					_IrisRestAPI = new IrisRestAPIAndroid();
				}
				return _IrisRestAPI;
			}
		}

		public bool Login(String userName, String password)
		{
			this.username = userName;
			this.password = password;

			HttpStatusCode status = HttpStatusCode.OK;
			string postString = String.Format("grant_type=password&username={0}&password={1}", WebUtility.HtmlEncode(userName), WebUtility.HtmlEncode(password));

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(String.Format("{0}/v1/login", UrlAPI)));

			request.ServerCertificateValidationCallback = ValidateServerCertificate;
			request.ClientCertificates.Add(Certificate);
			request.Timeout = (int)Timeout.TotalMilliseconds;
			request.Method = "POST";

			byte[] bytes = Encoding.UTF8.GetBytes(postString);

			using (Stream requestStream = request.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Flush();
			}

			try
			{
				HttpWebResponse httpResponse = (HttpWebResponse)(request.GetResponse());
				string json;
				using (Stream responseStream = httpResponse.GetResponseStream())
				{
					json = new StreamReader(responseStream).ReadToEnd();
				}
				Token = json;
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					WebException exc = ex as WebException;

					if (exc.Response is HttpWebResponse)
					{
						HttpWebResponse response = (HttpWebResponse)exc.Response;

						if (response.StatusCode == HttpStatusCode.InternalServerError)
						{
							throw new SecurityException("Fail to connect to the server.", ex);
						}
						else if (response.StatusCode == HttpStatusCode.BadRequest)
						{
							throw new SecurityException("Username/Password is invalid.", ex);
						}
					}
				}

				throw new SecurityException(ex.Message, ex);
			}

			return status == HttpStatusCode.OK;

		}

		public List<Camera> GetCamerasList()
		{
			List<Camera> cameras = null;

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(String.Format("{0}{1}", UrlAPI, "/v1/getcameraslist")));

			request.ServerCertificateValidationCallback = ValidateServerCertificate;
			request.ClientCertificates.Add(Certificate);
			request.Timeout = (int)Timeout.TotalMilliseconds;
			request.Method = "GET";
			request.Headers.Add("Authorization", String.Format("Bearer {0}", Token));

			HttpWebResponse webResponse = null;

			try
			{
				webResponse = (HttpWebResponse)(request.GetResponse());
				string json;
				using (Stream responseStream = webResponse.GetResponseStream())
				{
					json = new StreamReader(responseStream).ReadToEnd();
				}
				cameras = JsonConvert.DeserializeObject<List<Camera>>(json, Jss());
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					WebException exc = ex as WebException;
					if (exc.Response.Headers["Error"].ToLower() == "token expired" || exc.Response.Headers["Error"].ToLower() == "invalid token")
					{
						try
						{
							Login(this.username, this.password);
							return GetCamerasList();
						}
						catch { }
					}
				}
				throw new SecurityException(ex.Message, ex);
			}

			return cameras;
		}

		public List<PlaybackItem> GetSequences(PlaybackFilter filter)
		{

			List<PlaybackItem> list = new List<PlaybackItem>();

			string postString = JsonConvert.SerializeObject(filter);

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(String.Format("{0}/v1/getsequences", UrlAPI)));

			request.ServerCertificateValidationCallback = ValidateServerCertificate;
			request.ClientCertificates.Add(Certificate);
			request.Timeout = (int)Timeout.TotalMilliseconds;
			request.Method = "POST";
			request.Headers.Add("Authorization", String.Format("Bearer {0}", Token));

			byte[] bytes = Encoding.UTF8.GetBytes(postString);

			using (Stream requestStream = request.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Flush();
			}

			try
			{
				var webResponse = (HttpWebResponse)(request.GetResponse());
				string json;
				using (Stream responseStream = webResponse.GetResponseStream())
				{
					json = new StreamReader(responseStream).ReadToEnd();
				}
				list = JsonConvert.DeserializeObject<List<PlaybackItem>>(json, IrisRestAPIAndroid.Jss());
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					WebException exc = ex as WebException;
					if (exc.Response.Headers["Error"].ToLower() == "token expired" || exc.Response.Headers["Error"].ToLower() == "invalid token")
					{
						try
						{
							Login(this.username, this.password);
							return GetSequences(filter);
						}
						catch { }
					}
				}
				throw new SecurityException(ex.Message, ex);
			}

			return list;

		}


		/// <summary>
		/// for testing purpose only, accept any dodgy certificate... 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="certificate"></param>
		/// <param name="chain"></param>
		/// <param name="sslPolicyErrors"></param>
		/// <returns></returns>
		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

	}	
}
