using SendGrid.Helpers.Errors.Model;
using Microsoft.AspNetCore.Mvc;
using Externo.Data.Dtos;
using AutoMapper;
using Externo.Data.Daos;
using System.ComponentModel.DataAnnotations;
using Externo.Services;
using Externo.Util;

namespace Externo.Controllers
{
    /// <summary>
    /// Controlador responsável pela implementação dos endpoints associadas a email
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Tags("Externo")]
    public class EnviarEmailController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly EmailMensagemDao emailDao;
        private readonly EmailAPI emailAPI;

        /// <summary>
        /// Cria os objetos necessários para o controlador
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="_emailDao"></param>
        /// <param name="_emailAPI"></param>
        public EnviarEmailController(IMapper _mapper, EmailMensagemDao _emailDao, EmailAPI _emailAPI)
        {
            mapper = _mapper;
            emailDao = _emailDao;
            emailAPI = _emailAPI;
        }
        /// <summary> Notificar via email</summary>
        /// <param name="email"></param>
        /// <returns>Um objeto IActionResult que representa o resultado da operação de envio de email. Se o email for 
        /// enviado com sucesso, o método retorna um objeto Ok(). Caso contrário, ele retorna um objeto BadRequest().</returns>
        /// <response code="200">Externo solicitada</response>
        /// <response code="404">E-mail não existe</response>
        /// <response code="422">E-mail com formato inválido</response>
        /// <response code="500">Erro interno na aplicação</response>
        [HttpPost]
        [Route("enviarEmail")]
        [ProducesResponseType(200, Type = typeof(ReadEmailDto))]
        [ProducesResponseType(404, Type = typeof(ReadErroDto))]
        [ProducesResponseType(422, Type = typeof(ReadErroDto))]
        [ProducesResponseType(500, Type = typeof(ReadErroDto))]
        public async Task<IActionResult> EnviarEmail([FromBody, Required] CreateEmailDto requisicao)
        {
            try
            {
                // Validacao Formato de EmailMensagem
                if (!new EmailAddressAttribute().IsValid(requisicao.Email))
                {
                    throw new FormatException();
                }

                var response = await emailAPI.EnviaEmail(requisicao);

                if (response.IsSuccessStatusCode)
                {
                    // endereço de email válido e entregue com sucesso
                    var emailResposta = mapper.Map<ReadEmailDto>(emailDao.Add(requisicao.Assunto,
                                                                 requisicao.Email,
                                                                 requisicao.Mensagem));
                    emailResposta.Email = requisicao.Email;
                    return Ok(emailResposta);
                }
                else
                {
                    // EmailMensagem não pode ser entregue
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    new ReadErroDto
                    {
                        Codigo = Erro.InternoCod,
                        Mensagem = Erro.InternoMsg
                    });
                }
            }
            catch (NotFoundException)
            {
                return NotFound(new ReadErroDto
                {
                    Codigo = Erro.NotFindEmailCod,
                    Mensagem = Erro.NotFindEmailMsg
                });
            }
            catch (FormatException)
            {
                return UnprocessableEntity(new ReadErroDto
                {
                    Codigo = Erro.EmailFormatInvalidCod,
                    Mensagem = Erro.EmailFormatInvalidMsg
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ReadErroDto
                    {
                        Codigo = Erro.InternoCod,
                        Mensagem = Erro.InternoMsg
                    });
            }
        }

    }
}