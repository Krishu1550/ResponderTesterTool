using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoResponderPackage
{
	public static  class ResponseEndpointService
	{

		public const string filePath = "Response.json";

		public static bool IsEnable = false;


		public static async void CreateResponse(List< ApiDocumentation> ResponseEndpoint)
		{

		
	
			using (StreamWriter sw = new StreamWriter(filePath)) 
			{


				string json = JsonConvert.SerializeObject(ResponseEndpoint);

				sw.WriteLine(json);
				
			}



		}

		public static void UpdateResponse(Dictionary<string, object> ResponseEndpoint) 
		{ 
		
		
		
		}

		public static List<ApiDocumentation> GetResponse() {


			List<ApiDocumentation> responseEndpoint;
			using (StreamReader r = new StreamReader(filePath))
			{

				string json = r.ReadToEnd();	

			    responseEndpoint=  JsonConvert.DeserializeObject<List<ApiDocumentation>>(json);

				return responseEndpoint;
			}


		
		}

	}
}
