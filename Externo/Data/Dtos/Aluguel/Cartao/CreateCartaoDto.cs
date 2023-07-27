using Externo.Data.Validacao;
using Externo.Util;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Externo.Data.Dtos
{
    [SwaggerSchema(Title = SchemaNames.NovoCartao)]
    public class CreateCartaoDto
    {
        [Required]
        [ModelBinder(Name = Erro.NomeCod)]
        [StringLength(60, MinimumLength = 2, ErrorMessage = Erro.NomeMsg)]
        public string NomeTitular { get; set; }

        [Required]
        [ModelBinder(Name = Erro.NumCardCod)]
        [RegularExpression(@"\d+", ErrorMessage = Erro.NumCardMsg)]
        [StringLength(16, ErrorMessage = Erro.NumCardMsg)]
        public string Numero { get; set; }

        [Required]
        [ModelBinder(Name = Erro.DataCod)]
        [MyData]
        public string Validade { get; set; }

        [Required]
        [ModelBinder(Name = Erro.CvvCod)]
        [RegularExpression(@"\d{3,4}", ErrorMessage = Erro.CvvMsg)]
        //[DataType(DataType.c, ErrorMessage = Erro.CvvMsg)]
        public string Cvv { get; set; }
    }
}
