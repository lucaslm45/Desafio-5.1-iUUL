using AutoMapper;
using Externo.Data.Daos;
using Externo.Data.Dtos;
using Externo.Models;
using Externo.Services;
using Externo.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using NSwag.Annotations;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace Externo.Controllers
{
    /// <summary>
    /// Controlador responsável pela implementação dos endpoints associadas a cobranças
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Tags("Externo")]
    public class CobrancaController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly CobrancaDao cobrancaDao;
        private readonly PagamentoAPI pagamentoAPI;
        private readonly AluguelAPI aluguelAPI;
        private readonly EmailAPI emailAPI;

        /// <summary>
        /// Cria os objetos necessários para o controlador
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="_cobrancaDao"></param>
        /// <param name="_pagamentoAPI"></param>
        /// <param name="_aluguelAPI"></param>
        /// <param name="_emailAPI"></param>
        public CobrancaController(IMapper _mapper, CobrancaDao _cobrancaDao, PagamentoAPI _pagamentoAPI, AluguelAPI _aluguelAPI, EmailAPI _emailAPI)
        {
            mapper = _mapper;
            cobrancaDao = _cobrancaDao;
            pagamentoAPI = _pagamentoAPI;
            aluguelAPI = _aluguelAPI;
            emailAPI = _emailAPI;
        }
        /// <summary>
        /// Realizar cobrança
        /// </summary>
        /// <param name="cobrancaDto"></param>
        /// <returns></returns>
        /// <response code="200">Cobrança solicitada</response>
        /// <response code="422">Dados Inválidos</response>
        /// <response code="500">Erro interno na aplicação</response>
        [HttpPost]
        [Route("cobranca")]
        [ProducesResponseType(200, Type = typeof(ReadCobrancaDto))]
        [ProducesResponseType(422, Type = typeof(ReadErroDto))]
        [ProducesResponseType(500, Type = typeof(ReadErroDto))]
        public async Task<IActionResult> AdicionaCobranca([FromBody, Required] CreateCobrancaDto cobrancaDto)
        {
            var HoraSolicitacao = DateTime.UtcNow;

            try
            {
                // Aluguel: /cartaoDeCredito/{idCiclista}:
                var response = aluguelAPI.GetCartaoCiclista(cobrancaDto.Ciclista).Result;

                var content = await response.Content.ReadAsStringAsync();

                if (content == "")
                    throw new ArgumentNullException();

                if (!response.IsSuccessStatusCode)
                {
                    var result = new ContentResult
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content,
                        ContentType = "application/json"
                    };

                    return result;
                }

                var cartaoCiclista = JsonConvert.DeserializeObject<ReadCartaoDto>(content);

                var statusCobranca = pagamentoAPI.ProcessaPagamento(cobrancaDto.Valor, cartaoCiclista);

                if (statusCobranca == "succeeded")
                {
                    var cobrancaResposta = mapper.Map<ReadCobrancaDto>(cobrancaDao.Add(cobrancaDto.Ciclista,
                                                                                       cobrancaDto.Valor,
                                                                                       EStatusCobranca.Paga,
                                                                                       HoraSolicitacao));

                    return Ok(cobrancaResposta);
                }
                else
                {
                    return UnprocessableEntity(new ReadErroDto
                    {
                        Codigo = Erro.NotPagCod,
                        Mensagem = Erro.NotPagMsg
                    });
                }
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

        /// <summary>
        /// Processa todas as cobranças atrasadas colocadas em fila previamente.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Processamento concluído com sucesso</response>
        /// <response code="422">Dados Inválidos</response>
        /// <response code="500">Erro interno na aplicação</response>
        [HttpPost]
        [Route("processaCobrancasEmFila")]
        [ProducesResponseType(422, Type = typeof(List<ReadErroDto>))]
        [ProducesResponseType(500, Type = typeof(ReadErroDto))]
        public async Task<IActionResult> ProcessaCobrancasEmFila()
        {
            var errosResposta = new List<ReadErroDto>();
            try
            {
                var cobrancasPendentes = cobrancaDao.GetFilaPendente();
                if (cobrancasPendentes.Count == 0)
                    return Ok("Processamento concluído com sucesso");

                foreach (var cobranca in cobrancasPendentes)
                {
                    // Aluguel: /cartaoDeCredito/{idCiclista}:
                    var response = aluguelAPI.GetCartaoCiclista(cobranca.Ciclista).Result;

                    var content = await response.Content.ReadAsStringAsync();

                    if (content == "" || !response.IsSuccessStatusCode)
                    {
                        var erroResposta = new ReadErroDto
                        {
                            Codigo = Erro.NotFindCardCod,
                            Mensagem = $"{Erro.NotFindCardMsg}{cobranca.Ciclista}"
                        };
                        errosResposta.Add(erroResposta);
                        continue;
                    }

                    var cartaoCiclista = JsonConvert.DeserializeObject<ReadCartaoDto>(content);

                    // Processa Pagamento da Cobrança
                    var statusCobranca = pagamentoAPI.ProcessaPagamento(cobranca.Valor, cartaoCiclista);

                    if (statusCobranca == "succeeded")
                    {
                        // Reseta response e content
                        content = "";
                        response = null;

                        cobranca.HoraFinalizacao = DateTime.UtcNow;
                        cobranca.Status = EStatusCobranca.Paga;
                        cobrancaDao.Update(cobranca);

                        // Recuperar email ciclista

                        // Aluguel: Get /ciclista/{idCiclista}:
                        response = aluguelAPI.GetCiclista(cobranca.Ciclista).Result;

                        content = await response.Content.ReadAsStringAsync();

                        if (content == "" || !response.IsSuccessStatusCode)
                        {
                            var erroResposta = new ReadErroDto
                            {
                                Codigo = Erro.NotFindCiclisEmailCod,
                                Mensagem = $"{Erro.NotFindCiclisEmailMsg}{cobranca.Ciclista}."
                            };

                            errosResposta.Add(erroResposta);
                            continue;
                        }

                        var ciclista = JsonConvert.DeserializeObject<Ciclista>(content);

                        // Enviar Email para o Ciclista
                        var emailResponse = await emailAPI.EnviaEmail(new CreateEmailDto
                        {
                            Assunto = "Cobrança em atraso",
                            Mensagem = $"O valor de cobrança atrasada de {cobranca.Valor} referente a devolução foi cobrado com sucesso.",
                            Email = ciclista.email
                        });

                        if (!emailResponse.IsSuccessStatusCode)
                        {
                            var erroResposta = new ReadErroDto
                            {
                                Codigo = Erro.EmailNotSentCod,
                                Mensagem = $"{Erro.EmailNotSentMsg}{cobranca.Ciclista}."
                            };
                            errosResposta.Add(erroResposta);
                        }
                    }
                    else
                    {
                        var erroResposta = new ReadErroDto
                        {
                            Codigo = Erro.CobrancaNaoFeitaCod,
                            Mensagem = $"{Erro.CobrancaNaoFeitaMsg}{cobranca.Ciclista}."
                        };
                        errosResposta.Add(erroResposta);
                    }
                }
                if (errosResposta.Count > 0)
                    return UnprocessableEntity(errosResposta);

                return Ok("Processamento concluído com sucesso.");
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

        /// <summary>
        /// Inclui cobrança na fila de cobrança. Cobranças na fila serão cobradas de tempos em tempos.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Cobrança Incluida</response>
        /// <response code="422">Dados Inválidos</response>
        /// <response code="500">Erro interno na aplicação</response>
        [HttpPost]
        [Route("filaCobranca")]
        [ProducesResponseType(200, Type = typeof(ReadCobrancaDto))]
        [ProducesResponseType(422, Type = typeof(ReadErroDto))]
        [ProducesResponseType(500, Type = typeof(ReadErroDto))]
        public IActionResult AdicionaCobrancaNaFila([FromBody, Required] CreateCobrancaDto cobrancaDto)
        {
            var HoraSolicitacao = DateTime.UtcNow;

            try
            {
                var cobrancaResposta = mapper.Map<ReadCobrancaDto>(cobrancaDao.Add(cobrancaDto.Ciclista,
                                                                   cobrancaDto.Valor,
                                                                   EStatusCobranca.Pendente,
                                                                   HoraSolicitacao));

                return Ok(cobrancaResposta);
            }
            catch (NpgsqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ReadErroDto
                    {
                        Codigo = Erro.InternoCod,
                        Mensagem = Erro.InternoMsg
                    });
            }
            catch (Exception)
            {
                return UnprocessableEntity(new ReadErroDto
                {
                    Codigo = Erro.DadosInvalidCod,
                    Mensagem = Erro.DadosInvalidMsg
                });
            }
        }
        /// <summary>
        /// Obter cobrança
        /// </summary>
        /// <param name="idCobranca"></param>
        /// <returns></returns>
        /// <response code="200">Cobrança</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro interno na aplicação</response>
        [HttpGet]
        [Route("cobranca/{idCobranca}")]
        [ProducesResponseType(200, Type = typeof(ReadCobrancaDto))]
        [ProducesResponseType(404, Type = typeof(ReadErroDto))]
        [ProducesResponseType(500, Type = typeof(ReadErroDto))]
        public IActionResult RecuperaCobrancaPorId(Guid idCobranca)
        {
            try
            {
                var cobranca = cobrancaDao.GetById(idCobranca);

                if (cobranca == null)
                    return NotFound(new ReadErroDto
                    {
                        Codigo = Erro.NotCobrancaCod,
                        Mensagem = Erro.NotCobrancaMsg
                    });

                var cobrancaDto = mapper.Map<ReadCobrancaDto>(cobranca);

                return Ok(cobrancaDto);
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
        /// <summary>
        /// Valida um cartão de crédito
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Cartão válido</response>
        /// <response code="422">Dados inválidos</response>
        /// <response code="500">Erro interno na aplicação</response>
        [HttpPost]
        [Route("validaCartaoDeCredito")]
        [ProducesResponseType(422, Type = typeof(ReadErroDto))]
        [ProducesResponseType(500, Type = typeof(ReadErroDto))]
        public async Task<IActionResult> ValidaCartaoDeCredito([FromBody, Required] CreateCartaoDto cartao)
        {

            try
            {
                var service = new TokenService(StripeConfiguration.StripeClient);
                var token = service.Create(new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Name = cartao.NomeTitular,
                        Number = cartao.Numero,
                        ExpMonth = cartao.Validade.Substring(5, 2),//.Month.ToString(),
                        ExpYear = cartao.Validade.Substring(0, 4),//.Year.ToString(),
                        Cvc = cartao.Cvv
                    }
                });
                if (token.Card == null || token.Card.Fingerprint == null)
                    throw new Exception();

                else
                {
                    return Ok("Cartão válido");
                }
            }
            catch (StripeException ex)
            {
                if (ex.Message.Contains("\'Your card\'s expiration"))
                    return UnprocessableEntity(new ReadErroDto
                    {
                        Codigo = Erro.CardVencCod,
                        Mensagem = Erro.CardVencMsg
                    });

                else if (ex.Message.Contains("Your card number is incorrect"))
                    return UnprocessableEntity(new ReadErroDto
                    {
                        Codigo = Erro.NumCardCod,
                        Mensagem = Erro.NumCardMsg
                    });

                return UnprocessableEntity(new ReadErroDto
                {
                    Codigo = Erro.CardCod,
                    Mensagem = Erro.CardMsg
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
