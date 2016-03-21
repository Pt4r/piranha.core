﻿/*
 * Copyright (c) 2016 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.dnx
 * 
 */

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace Piranha.AspNet
{
	public class PageMiddleware : MiddlewareBase
	{
		/// <summary>
		/// Creates a new middleware instance.
		/// </summary>
		/// <param name="next">The next middleware in the pipeline</param>
		public PageMiddleware(RequestDelegate next) : base(next) { }

		/// <summary>
		/// Invokes the middleware.
		/// </summary>
		/// <param name="context">The current http context</param>
		/// <returns>An async task</returns>
		public override async Task Invoke(HttpContext context) {
			var url = context.Request.Path.HasValue ? context.Request.Path.Value : "";

			if (!String.IsNullOrWhiteSpace(url) && url.Length > 1) {
				var segments = url.Substring(1).Split(new char[] { '/' });

				var include = segments.Length;

				// Scan for the most unique slug
				for (var n = include; n > 0; n--) {
					var slug = segments.Subset(0, n).Implode("/");
					var page = api.Pages.GetBySlug(slug);

					if (page != null) {
						var route = page.Route;

						if (n < include) {
							route += "/" + segments.Subset(n).Implode("/");
						}

						// Set path
						context.Request.Path = new PathString(route);

						// Set query
						if (context.Request.QueryString.HasValue) {
							context.Request.QueryString = new QueryString(context.Request.QueryString.Value + "&id=" + page.Id);
						} else context.Request.QueryString = new QueryString("?id=" + page.Id);
					}
				}
			}
			await next.Invoke(context);
		}
	}
}
