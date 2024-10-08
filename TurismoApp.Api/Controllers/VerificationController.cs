﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common.DTO.ClientesDtos;

namespace TurismoApp.Api.Controllers
{
    [ApiController]
    [Route("api/Verificacion")]
    public class VerificacionController : ControllerBase
    {
        private readonly IClienteApplication _clienteApplication;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public VerificacionController(IClienteApplication clienteApplication, EmailService emailService, IMapper mapper)
        {
            _clienteApplication = clienteApplication;
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpPost("Enviar-verificación")]
        public async Task<IActionResult> SendVerificationEmail(int clientId)
        {
            var clienteResult = await _clienteApplication.GetClienteVerificationByIdAsync(clientId);
            if (!clienteResult.IsValid)
            {
                return NotFound("Cliente no encontrado.");
            }

            var cliente = _mapper.Map<ClienteDto>(clienteResult.Data);
            if (cliente == null)
            {
                return BadRequest("Datos del cliente no válidos.");
            }
            Console.WriteLine($"Cliente ID: {cliente.Id}, Verificado: {cliente.Verificado}, Token: {cliente.VerificacionToken}");

            if (cliente.Verificado)
            {
                return BadRequest("El cliente ya está verificado.");
            }

            var token = Guid.NewGuid().ToString();
            var verificationLink = Url.Action(nameof(ConfirmEmail), "Verificacion", new { clientId, token }, Request.Scheme);

            var saveTokenResult = await _clienteApplication.SaveVerificationTokenAsync(clientId, token);
            if (!saveTokenResult.IsValid)
            {
                return BadRequest(saveTokenResult.Message);
            }

            await _emailService.SendVerificationEmailAsync(cliente.Correo, verificationLink);

            return Ok("Correo de verificación enviado.");
        }

        [HttpGet("Verificacion")]
        public async Task<IActionResult> ConfirmEmail(int clientId, string token)
        {
            var isTokenValid = await _clienteApplication.VerifyTokenAsync(clientId, token);
            if (!isTokenValid)
            {
                return BadRequest("Token de verificación no válido.");
            }

            var clienteResult = await _clienteApplication.GetClienteByIdAsync(clientId);
            if (!clienteResult.IsValid)
            {
                return NotFound("Cliente no encontrado.");
            }

            var cliente = _mapper.Map<VerifyClienteDto>(clienteResult.Data);
            if (cliente == null)
            {
                return BadRequest("Datos del cliente no válidos.");
            }

            if (cliente.Verificado)
            {
                return BadRequest("El cliente ya está verificado.");
            }

            cliente.Verificado = true;
            cliente.FechaVerificacion = DateTime.Now;
            var updateResult = await _clienteApplication.UpdateClienteVerificationAsync(clientId, cliente);
            if (!updateResult.IsValid)
            {
                return BadRequest(updateResult.Message);
            }

            return Ok("Correo verificado con éxito.");
        }
    }
}