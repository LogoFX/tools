using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.lient.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class WarehouseProvider : IWarehouseProvider
    {
        private readonly RestClient _client;

        public WarehouseProvider(RestClient client)
        {
            _client = client;
        }

        public IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            var restRequest = new RestRequest("api/warehouse", Method.GET) { RequestFormat = DataFormat.Json };
            var response = _client.Execute<List<WarehouseItemDto>>(restRequest);            
            return response.Data;            
        }

        public bool DeleteWarehouseItem(Guid id)
        {
            var restRequest = new RestRequest($"api/warehouse/{id}", Method.DELETE) { RequestFormat = DataFormat.Json };
            var response = _client.Execute(restRequest);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public bool UpdateWarehouseItem(WarehouseItemDto dto)
        {
            var restRequest = new RestRequest($"api/warehouse/{dto.Id}", Method.PUT) { RequestFormat = DataFormat.Json };            
            restRequest.AddBody(dto);
            var response = _client.Execute(restRequest);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public void CreateWarehouseItem(WarehouseItemDto dto)
        {
            var restRequest = new RestRequest("api/warehouse", Method.POST) { RequestFormat = DataFormat.Json };
            restRequest.AddBody(dto);
            var response = _client.Execute(restRequest);            
        }
    }
}
