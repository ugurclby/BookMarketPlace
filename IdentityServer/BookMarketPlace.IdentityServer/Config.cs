﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace BookMarketPlace.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullperms"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullperms"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullperms"}},
            new ApiResource("resource_order"){Scopes={"order_fullperms"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullperms"}},
            new ApiResource("resource_fakePayment"){Scopes={"fakePayment_fullperms"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullperms"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                    new IdentityResources.Email(),
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullperms","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullperms","Photo API için full erişim"),
                new ApiScope("basket_fullperms","Basket API için full erişim"),
                new ApiScope("discount_fullperms","Discout Api için full erişim"),
                new ApiScope("order_fullperms","Order Api için full erişim"),
                new ApiScope("fakePayment_fullperms","FakePayment Api için full erişim"),
                new ApiScope("gateway_fullperms","Gateway Api için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 new Client()
                 {
                     ClientName="Asp.Net Core MVC",
                     ClientId="WebMvcClient",
                     ClientSecrets={new Secret("secret".Sha256())},
                     AllowedGrantTypes=GrantTypes.ClientCredentials,
                     AllowedScopes={ "catalog_fullperms", "photo_stock_fullperms", "gateway_fullperms", IdentityServerConstants.LocalApi.ScopeName }
                 },
                 new Client()
                 {
                     ClientName="Asp.Net Core MVC",
                     ClientId="WebMvcClientForUser",
                     AllowOfflineAccess=true,
                     ClientSecrets={new Secret("secret".Sha256())},
                     AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                     AllowedScopes={"basket_fullperms", "discount_fullperms","order_fullperms","fakePayment_fullperms","gateway_fullperms",
                         IdentityServerConstants.StandardScopes.Email,
                     IdentityServerConstants.StandardScopes.OpenId,
                     IdentityServerConstants.StandardScopes.Profile,
                     IdentityServerConstants.StandardScopes.OfflineAccess,
                         IdentityServerConstants.LocalApi.ScopeName,
                         "roles"
                     },
                     AccessTokenLifetime=1*60*60,
                     RefreshTokenExpiration = TokenExpiration.Absolute,
                     AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                     RefreshTokenUsage=TokenUsage.ReUse,

                 },
                    new Client()
                    {
                        ClientName="Token Exchange Client",
                        ClientId="TokenExchangeClient",
                        ClientSecrets={new Secret("secret".Sha256())},
                        AllowedGrantTypes=new []{"urn:ietf:params:oauth:grant-type:token-exchange"},
                        AllowedScopes={"discount_fullperms","fakePayment_fullperms","gateway_fullperms",
                     IdentityServerConstants.StandardScopes.OpenId,
                     }
                    }
            };
    }
}