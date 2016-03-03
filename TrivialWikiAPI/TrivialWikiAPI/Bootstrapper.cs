﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using TrivialWikiAPI.DatabaseModels;
using Nancy.Authentication.Stateless;
using Nancy.Security;

namespace TrivialWikiAPI
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            ConfigStatelessAuthentication(pipelines);
        }

        private void ConfigStatelessAuthentication(IPipelines pipelines)
        {
            var config = new StatelessAuthenticationConfiguration(ctx => AuthenticateUser(ReadAuthToken(ctx)));
            StatelessAuthentication.Enable(pipelines, config);
        }

        private static string ReadAuthToken(NancyContext ctx)
        {
            string token = ctx.Request.Headers.Authorization;
            if (string.IsNullOrEmpty(token))
            {
                token = ctx.Request.Query.token;
            }
            return token;
        }

        private static IUserIdentity AuthenticateUser(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            using(var databaseContext = new DatabaseContext())
            {
                var user = databaseContext.Users
                    .Include("Roles")
                    .Where(u => u.SecurityToken == token)
                    .FirstOrDefault();

                if(user == null)
                {
                    return null;
                }
                var claims = user.Roles.Select(r => r.Name).ToList();
                user.Claims = claims;

                return user;
            }
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
        }
    }
}