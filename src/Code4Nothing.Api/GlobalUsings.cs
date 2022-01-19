global using Code4Nothing.Api;
global using HealthChecks.UI.Client;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.IdentityModel.Tokens.Jwt;
global using Code4Nothing.Infrastructure.Persistence;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using Polly;
global using Code4Nothing.Api.Middlewares;
global using System.Security.Claims;
global using Code4Nothing.Application.Common.Interfaces;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Versioning;
global using System.Text.Json;
global using System.Net;
global using Code4Nothing.Api.Models;
global using Code4Nothing.Application;
global using Code4Nothing.Infrastructure;
global using MediatR;
global using Code4Nothing.Application.TodoItems.Commands;
global using Code4Nothing.Domain.TodoItem.Entities;
global using Code4Nothing.Api.Services;
global using Code4Nothing.Application.Common.Models;
global using Code4Nothing.Application.TodoItems.Queries;