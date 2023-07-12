using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoResponderPackage
{
	public class ApiDocumentationGenerator: IApiEndpointService
	{

		private readonly IApiDescriptionGroupCollectionProvider _provider;
		public ApiDocumentationGenerator(IApiDescriptionGroupCollectionProvider provider)
		{
			_provider = provider;
		}


		public List<ApiDocumentation> GetAllEndpoints()
		{
			var apiDescriptions =  _provider.ApiDescriptionGroups.Items
				.SelectMany( gr=> gr.Items).Where(
				des => !des.ActionDescriptor.RouteValues["controller"].StartsWith("Microsoft")
				).ToList();

			var apiDocs = new List<ApiDocumentation>();

			foreach ( var apiDescription in apiDescriptions )
			{
				var responseModel = GetResponseType(apiDescription);
				var requestModel = GetRequestType(apiDescription);

				var documentation = new ApiDocumentation()
				{
					HttpMethod= apiDescription.HttpMethod,
					Path= apiDescription.RelativePath,
					RequestModel = requestModel,
					ResponseModel = responseModel
				};

				apiDocs.Add(documentation);

			}

			return apiDocs;
		}
				
		public object GetResponseType(ApiDescription apiDescription)
		{

			var responseType = apiDescription.SupportedResponseTypes.FirstOrDefault();
			if(responseType?.Type != null) 
			{
	

				return CreateInstance(responseType.Type);
			
			}
			return null;

		}

		public object GetRequestType(ApiDescription apiDescription) 
		{ 
		//	var responseType= apiDescription.ParameterDescriptions
		//		.Where(des=> des.Source.Id.Equals("Body", StringComparison.OrdinalIgnoreCase)).
		//		Select(des=>des.ModelMetadata.ContainerType).FirstOrDefault();

			var responseType = apiDescription.ParameterDescriptions.FirstOrDefault();
			if (responseType != null)
			{
				if (responseType.Type != null)
				{
					return CreateInstance(responseType.Type);
				}
			}

			return null;

		}

		public object CreateInstance(Type type)
		{
			if (type == typeof(string))
			{
				return string.Empty;
			}
			else if (type == typeof(bool))
			{
				return false;
			}
			else if (type.IsGenericType)
			{

				var genericType = type.GetGenericArguments()[0];
				var listType = typeof(List<>).MakeGenericType(genericType);
				var listIntance = Activator.CreateInstance(listType);

				var defaultValue = CreateInstance(genericType); 
					//Activator.CreateInstance(genericType);

				listType.GetMethod("Add")?.Invoke(listIntance, new[] { defaultValue });

				return listIntance;
			}
			else if(type == typeof(void))
			{
				return null;
			}

			else
			{
				return Activator.CreateInstance(type);
			}

		}


	}

	public class ApiDocumentation
	{
		public string HttpMethod { get; set; }
		public string Path { get; set; }
		public object RequestModel { get; set; }
		public object ResponseModel { get; set; }	
	}

	public interface IApiEndpointService
	{
		List<ApiDocumentation> GetAllEndpoints();
	}

	

}
