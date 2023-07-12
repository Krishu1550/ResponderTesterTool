using AutoResponderPackage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoResponder.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EndpointsController : ControllerBase
	{

		private readonly IApiEndpointService _endpointService;

        public EndpointsController(IApiEndpointService apiEndpointService)
        {
            _endpointService = apiEndpointService;
        }
		[HttpGet]
		public IEnumerable<ApiDocumentation> GetAllEndpoints() 
		{ 

		 var endpoints = _endpointService.GetAllEndpoints();	

		 return endpoints;	
		}

		[HttpGet]
		[Route("TesterMode")]
		public void EnableTesterMode( bool IsEnable)
		{

			ResponseEndpointService.IsEnable = IsEnable;


		}
		[HttpPost]
		public void DefinedRespons([FromBody] Object responses)
		{
			
			var endpointResponse = JsonConvert.DeserializeObject<List<ApiDocumentation>>(responses.ToString());

			

			ResponseEndpointService.CreateResponse(endpointResponse);
		}


    }
}
