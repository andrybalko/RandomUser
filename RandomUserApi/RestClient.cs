using Newtonsoft.Json;
using RandomUserApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RandomUserApi
{
	public interface IRestClient
	{
		Request CreateRequest();
		Task<User[]> MakeRequestAsync(Request request);
	}

	public class RestClient : IRestClient
	{
		private HttpClient client;
		private RestClientConfig config;


		private Request request;

		public RestClient(RestClientConfig config)
		{
			this.config = config;
			
			//TODO can be extracted to separate method and added to the interface declaration
			client = new HttpClient
			{
				BaseAddress = new Uri("https://randomuser.me")
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

		}

		public Request CreateRequest()
		{
			return new Request(config);
		}

		public async Task<User[]> MakeRequestAsync(Request request)
		{

			var res = await client.GetAsync(request.GetPath());
			res.EnsureSuccessStatusCode();
			
			string body = await res.Content.ReadAsStringAsync();
			Debug.WriteLine(body);

			try
			{
				var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await res.Content.ReadAsStringAsync());

				if (string.IsNullOrEmpty(apiResponse.Error))
				{
					if (config.UseSeed)
					{
						config.Seed = apiResponse.Info.Seed;
					}

					return apiResponse.Users;
				}
				else
				{
					//TODO process error
				}
			}
			catch (Exception e)
			{
				throw;
			}
			return new User[0];
		}

	}

	public class RestClientConfig
	{
		public string BasePath => "/api";

		public string[] IncludeFields;
		private string seed;

		public string Seed { get => seed ?? ""; set => seed = value; }
		public bool UseSeed { get; set; }

		public RestClientConfig(string[] fields)
		{
			IncludeFields = fields;

		}

		public RestClientConfig()
		{
		}
	}

	public class Request
	{
		private readonly RestClientConfig config;

		private Dictionary<string, RequestSegment> _segments { get; set; }

		public Request(RestClientConfig config)
		{
			_segments = new Dictionary<string, RequestSegment>();
			this.config = config;
		}

		private Request AddSegment (RequestSegment seg)
		{
			_segments[seg.Name] = seg;
			return this;
		}


		public Request ForPage(int page)
		{
			AddSegment(new RequestSegment(RequestSegment.Page, page.ToString()));
			return this;
		}

		public Request ForSeed()
		{
			AddSegment(new RequestSegment(RequestSegment.Seed, config.Seed));
			return this;
		}

		public Request ForGender(string gender)
		{
			AddSegment(new RequestSegment(RequestSegment.Gender, gender));
			_segments.Remove(RequestSegment.Seed);
			return this;
		}

		public Request ForNationality(string nat)
		{
			AddSegment(new RequestSegment(RequestSegment.Nationality, nat));
			return this;
		}

		public Request Results(int usersPerPage = 10)
		{
			//make sure that number is in the allowed range 0..5000
			//defaulted to 10
			usersPerPage = Math.Max(1, usersPerPage);
			usersPerPage = Math.Min(usersPerPage, 5000);

			AddSegment(new RequestSegment(RequestSegment.Results, usersPerPage.ToString()));
			return this;
		}

		

		public string GetPath()
		{
			return config.BasePath + "?" + string.Join("&", _segments.Select(x => x.Value.Name + "=" + x.Value.Value).ToArray());
		}

		private string AppendSegment(string path, RequestSegment segment)
		{
			if (path.IndexOf("?") > -1)
			{
				//this means that at least 1 segment has been added to url
				return path += "&" + segment.ToString();
			}

			return path + "?" + segment;
		}

		

	}

	public class RequestSegment
	{
		public const string Page = "page";
		public const string IncludedFields = "inc";
		public const string Seed = "seed";
		public const string Results = "results";
		public const string Gender = "gender";
		public const string Nationality = "nat";



		private string _name;
		private string _value;

		public string Name => _name;
		public string Value => _value;


		public RequestSegment(string paramName, string value)
		{
			_name = paramName;
			_value = value;
		}

		public override string ToString()
		{
			return ConstructSegment(Name, Value);
		}

		private string ConstructSegment(string param, string value)
		{
			return param + "=" + value;
		}

		private string ConstructSegment(string param, string[] value)
		{
			return param + "=" + string.Join(",", value);
		}

	}

	public class RequestField
	{
		public const string Gender = "gender";
		public const string Name = "name";
		public const string Location = "location";
		public const string Email = "email";
		public const string Login = "login";
		public const string Registered = "registered";
		public const string Dob = "dob";
		public const string Phone = "phone";
		public const string Cell = "cell";
		public const string Id = "id";
		public const string Picture = "picture";
		public const string Nat = "nat";
	}
}
