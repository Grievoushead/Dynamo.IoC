﻿using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

// http://bradwilson.typepad.com/blog/2010/10/service-location-pt5-idependencyresolver.html

// Adds support for both Mvc and Web Api

namespace Dynamo.Ioc.Web
{
	public class DynamoDependencyResolver : IServiceProvider, System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
	{
		private readonly IResolver _resolver;

		public DynamoDependencyResolver(IResolver resolver)
		{
			if (resolver == null)
				throw new ArgumentNullException("resolver");

			_resolver = resolver;
		}

		public object GetService(Type serviceType)
		{
			object obj;
			_resolver.TryResolve(serviceType, out obj);
			return obj;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _resolver.TryResolveAll(serviceType);
		}

		public System.Web.Http.Dependencies.IDependencyScope BeginScope()
		{
			// Be able to switch between Container Scope (returning this) and "real" scope

			return new DynamoDependencyScope(_resolver.GetScope());	// or GetRequestScope() ?

			//return this;
		}

		public void Dispose()
		{
		}
	}
}