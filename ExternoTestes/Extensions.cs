using Externo.Data.Dtos;
using Externo.Util;
using ExternoTestes.Equipamento;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternoTestes
{
    public static class Extensions
    {
        public async static Task<IList<T>> CriaBicicletasAsync<T>(string url)
        {
            var objetos = new List<object>()
            {
                new
                {
                    Marca = "Marca Teste I",
                    Modelo = "Modelo Teste I",
                    Ano = "2000"
                },
                new
                {
                    Marca = "Marca Teste II",
                    Modelo = "Modelo Teste II",
                    Ano = "2001",
                },
                new
                {
                    Marca = "Marca Teste III",
                    Modelo = "Modelo Teste III",
                    Ano = "2002"
                }
            };

            return await SolicitaRequisicaoAsync<T>(url, HttpMethod.Post, null, objetos);
        }
        public static async Task<IList<T>> CriaTrancasAsync<T>(string url)
        {
            var objetos = new List<object>()
            {
                new
                {
                    anoDeFabricacao = "2020",
                    modelo = "Modelo Teste I"
                },
                new
                {
                    anoDeFabricacao = "2021",
                    modelo = "Modelo Teste II"
                },
                new
                {
                    anoDeFabricacao = "2022",
                    modelo = "Modelo Teste III"
                }
            };

            return await SolicitaRequisicaoAsync<T>(url, HttpMethod.Post, null, objetos);
        }
        public async static Task<T> CriaTotemAsync<T>(string url)
        {
            var objetos = new List<object>()
            {
                new
                {
                    localizacao = "Localizacao Teste I",
                }
            };

            var result = await SolicitaRequisicaoAsync<T>(url, HttpMethod.Post, null, objetos);

            return result.First();
        }
        public static async Task<IList<T>> SolicitaRequisicaoAsync<T>(string url,
                                                     HttpMethod method,
                                                     Guid? id = null,
                                                     IList<object>? objetos = null,
                                                     object? singleObj = null)
        {
            var arrange = new List<T>();
            var requestNotUse = id is null ? new HttpRequestMessage(method, url) :
                new HttpRequestMessage(method, url + id);

            if (objetos != null)
            {
                foreach (var obj in objetos)
                {
                    var json = JsonConvert.SerializeObject(obj);
                    var request = new HttpRequestMessage(requestNotUse.Method, requestNotUse.RequestUri);
                    request.Content = new StringContent(json, null, "application/json");
                    arrange.Add(await FazSolicitacaoHttp<T>(request));
                }
            }
            else if (singleObj != null)
            {
                var json = JsonConvert.SerializeObject(singleObj);
                var request = new HttpRequestMessage(requestNotUse.Method, requestNotUse.RequestUri);
                request.Content = new StringContent(json, null, "application/json");
                arrange.Add(await FazSolicitacaoHttp<T>(request));
            }
            else
                arrange.Add(await FazSolicitacaoHttp<T>(requestNotUse));

            return arrange;
        }
        private async static Task<T> FazSolicitacaoHttp<T>(HttpRequestMessage request)
        {
            using (var client = new HttpClient())
            {
                //var request = new HttpRequestMessage(forRequest.Method, forRequest.RequestUri);
                //request.Content = forRequest.Content;

                var response = await client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception();


                return JsonConvert.DeserializeObject<T>(content);
            }

        }
    }
}
