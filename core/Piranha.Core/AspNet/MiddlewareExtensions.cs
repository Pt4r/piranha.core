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
using Piranha;
using System;

public static class MiddlewareExtensions
{
	/// <summary>
	/// Uses the piranha middleware.
	/// </summary>
	/// <param name="builder">The current application builder</param>
	/// <param name="options">The startup options</param>
	/// <returns>The builder</returns>
	public static IApplicationBuilder UsePiranha(this IApplicationBuilder builder, Startup options = Startup.All) {
		return builder.UsePiranha(null, options);
	}

	/// <summary>
	/// Uses the piranha middleware.
	/// </summary>
	/// <param name="builder">The current application builder</param>
	/// <param name="config">The Piranha app config</param>
	/// <param name="options">The startup options</param>
	/// <returns>The builder</returns>
	public static IApplicationBuilder UsePiranha(this IApplicationBuilder builder, Action<AppConfig> config, Startup options = Startup.All) {
		App.Init(config);

		if (options.HasFlag(Startup.Pages) || options.HasFlag(Startup.All))
			builder = builder.UseMiddleware<Piranha.AspNet.PageMiddleware>();
		if (options.HasFlag(Startup.Posts) || options.HasFlag(Startup.All))
			builder = builder.UseMiddleware<Piranha.AspNet.PostMiddleware>();
		if (options.HasFlag(Startup.StartPage) || options.HasFlag(Startup.All))
			builder = builder.UseMiddleware<Piranha.AspNet.StartPageMiddleware>();

		return builder;
	}
}
