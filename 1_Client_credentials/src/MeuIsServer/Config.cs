// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Esse é o arquivo de configuração de recursos e clientes do IdentityServer‎

/* O trabalho de um serviço de token OpenID Connect/OAuth é controlar o acesso aos recursos de um sistema.

Existem dois tipos de recursos:

‎Recursos de identidade(identity resources):‎‎ representam reivindicações sobre um usuário, como ID de usuário,
nome etc.
‎        
Recursos de API(API resources):‎‎ representam funcionalidades que o cliente deseja acessar. */

using IdentityServer4.Models;
using System.Collections.Generic;

namespace MeuIsServer
{
    public static class Config
    {
        /* Define uma API como escopo. Os escopos representam algo que você deseja proteger e que
        os clientes desejam acessar. */
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1", "My API"),
            };

        /* Define os clientes. Os clientes representam aplicativos que podem solicitar tokens do 
        IdentityServer. */
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                /* A configuração de usuário abaixo necessita apenas do ClientId e do ClientSecrets 
                para autenticação */
                new Client {
                    // ID do cliente
                    ClientId = "client",
                    
                    // Segredo para autenticação.
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },

                    // As interações permitidas com o serviço de token(chamado de grantType).
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // Escopos que o cliente tem acesso.
                    AllowedScopes = { "api1" }
              }
            };
    }
}