using Newtonsoft.Json;
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace Externo.Util
{
    public static class CreateHttpContents
    {
        private static HttpContent CreateHttpContent(object value)
        {
            var json = JsonConvert.SerializeObject(value);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        public static HttpContent Email(string _email, string _assunto, string _message)
        {
            var objeto = new
            {
                email = _email,
                assunto = _assunto,
                mensagem = _message
            };

            return CreateHttpContent(objeto);
        }
        public static HttpContent Cobranca(string _ciclista, decimal _valor)
        {
            var objeto = new
            {
                valor = _valor,
                ciclista = _ciclista
            };

            return CreateHttpContent(objeto);
        }
        public static HttpContent Tranca(int _numero,
                                         string _localizacao,
                                         string _anoFabricacao,
                                         string _modelo,
                                         string _status)
        {
            var objeto = new
            {
                Numero = _numero,
                AnoDeFabricacao = _anoFabricacao,
                Modelo = _modelo,
                Localizacao = _localizacao,
                Status = _status
            };

            return CreateHttpContent(objeto);
        }
        public static HttpContent ValidaEmail(string _email)
        {
            var objeto = new
            {
                ciclista = _email
            };

            return CreateHttpContent(objeto);
        }
        public static HttpContent Bicicleta(string _marca, string _modelo, string _ano, int _numero, string _status)
        {
            var objeto = new
            {
                marca = _marca,
                modelo = _modelo,
                ano = _ano,
                numero = _numero,
                status = _status
            };

            return CreateHttpContent(objeto);
        }
    }
}
