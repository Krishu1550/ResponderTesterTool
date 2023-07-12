using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoResponderPackage
{
	public class ProxyResponseMiddleWare 
	{
		private readonly RequestDelegate next;

		public ProxyResponseMiddleWare(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{	
			if(ResponseEndpointService.IsEnable)
			{
				var path = context.Request.Path.Value;
				var method = context.Request.Method;
				var endpoints = ResponseEndpointService.GetResponse();
				foreach( ApiDocumentation endpoint in endpoints)
				{
					if("/"+endpoint.Path==path && endpoint.HttpMethod==method )
					{
						context.Response.StatusCode = 200;
						context.Response.ContentType = "application/json";
						var jsonResponse = JsonConvert.SerializeObject(endpoint.ResponseModel);
						context.Response.ContentLength= Encoding.UTF8.GetByteCount(jsonResponse);
                        await context.Response.WriteAsync(jsonResponse,Encoding.UTF8);
						await next(context);
					}
				}

				await next(context);
			}
			else
			{
				await next(context);	
			}
		
		}
	}
}
