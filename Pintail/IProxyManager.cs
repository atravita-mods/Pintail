using System;
using System.Diagnostics.CodeAnalysis;

namespace Nanoray.Pintail
{
    /// <summary>
    /// Represents a type responsible for creating and returning <see cref="IProxyFactory{}"/> instances.
    /// </summary>
    /// <typeparam name="Context">The context type used to describe the current proxy process. Use <see cref="Nothing"/> if not needed.</typeparam>
    public interface IProxyManager<Context>
    {
        /// <summary>
        /// Returns an existing <see cref="IProxyFactory{}"/> instance for the given proxy info.
        /// </summary>
        /// <param name="proxyInfo">Proxy info describing the <see cref="IProxyFactory{}"/> to return.</param>
        /// <returns>An existing <see cref="IProxyFactory{}"/> instance for the given proxy info, or `null` if one does not exist.</returns>
        IProxyFactory<Context>? GetProxyFactory(ProxyInfo<Context> proxyInfo);

        /// <summary>
        /// Returns an existing <see cref="IProxyFactory{}"/> instance for the given proxy info or creates and returns a new one.
        /// </summary>
        /// <param name="proxyInfo">Proxy info describing the <see cref="IProxyFactory{}"/> to return.</param>
        /// <returns>A <see cref="IProxyFactory{}"/> instance for the given proxy info.</returns>
        IProxyFactory<Context> ObtainProxyFactory(ProxyInfo<Context> proxyInfo);
	}

    public static class ProxyManagerExtensions
    {
        /// <summary>
        /// Returns a proxy instance for a given instance.
        /// </summary>
        /// <typeparam name="Context">The context type used to describe the current proxy process. Use <see cref="Nothing"/> if not needed.</typeparam>
        /// <typeparam name="TProxy">The interface type to proxy the instance to.</typeparam>
        /// <param name="self">Target of the extension method.</param>
        /// <param name="instance">The instance to create a proxy for.</param>
        /// <param name="targetContext">The context of the target instance.</param>
        /// <param name="proxyContext">The context of the proxy instance.</param>
        /// <returns>A proxy of the given instance.</returns>
        [return: NotNullIfNotNull("instance")]
        public static TProxy? ObtainProxy<Context, TProxy>(this IProxyManager<Context> self, object? instance, Context targetContext, Context proxyContext) where TProxy: class
        {
            if (instance is null)
                return null;

            var factory = self.ObtainProxyFactory(new ProxyInfo<Context>(
                target: new TypeInfo<Context>(targetContext, instance.GetType()),
                proxy: new TypeInfo<Context>(proxyContext, typeof(TProxy))
            ));
            return (TProxy)factory.ObtainProxy(self, instance);
        }

        /// <summary>
        /// Tries to return a proxy instance for a given instance (or unproxy it if it's already a proxy and the type would be accessible when doing so).
        /// </summary>
        /// <typeparam name="Context"></typeparam>
        /// <typeparam name="TProxy"></typeparam>
        /// <param name="self">Target of the extension method.</param>
        /// <param name="toProxy">The instance to create a proxy for (or to unproxy).</param>
        /// <param name="targetContext">The context of the target instance.</param>
        /// <param name="proxyContext">The context of the proxy instance.</param>
        /// <param name="proxy">The resulting proxy instance (or unproxied instance), if the (un)proxying succeeds.</param>
        /// <returns>`true` if the (un)proxying succeeds, `false` otherwise.</returns>
        public static bool TryProxy<Context, TProxy>(this IProxyManager<Context> self, object? toProxy, Context targetContext, Context proxyContext, out TProxy? proxy) where TProxy: class
        {
            if (toProxy is null)
            {
                proxy = null;
                return true;
            }

            try
            {
                foreach (Type interfaceType in toProxy.GetType().GetInterfacesRecursively(includingSelf: true))
                {
                    var unproxyFactory = self.GetProxyFactory(new ProxyInfo<Context>(
                        target: new TypeInfo<Context>(targetContext, typeof(TProxy)),
                        proxy: new TypeInfo<Context>(proxyContext, interfaceType)
                    ));
                    if (unproxyFactory is null)
                        continue;
                    if (unproxyFactory.TryUnproxy(toProxy, out object? targetInstance))
                    {
                        proxy = (TProxy?)targetInstance;
                        return true;
                    }
                }

                var proxyFactory = self.ObtainProxyFactory(new ProxyInfo<Context>(
                    target: new TypeInfo<Context>(targetContext, toProxy.GetType()),
                    proxy: new TypeInfo<Context>(proxyContext, typeof(TProxy))
                ));
                proxy = (TProxy)proxyFactory.ObtainProxy(self, toProxy);
                return true;
            }
            catch
            {
                proxy = null;
                return false;
            }
        }
    }

    public static class NothingProxyManagerExtensions
    {
        /// <summary>
        /// Returns a proxy instance for a given instance.
        /// </summary>
        /// <typeparam name="TProxy">The interface type to proxy the instance to.</typeparam>
        /// <param name="self">Target of the extension method.</param>
        /// <param name="instance">The instance to create a proxy for.</param>
        /// <returns>A proxy of the given instance.</returns>
        [return: NotNullIfNotNull("instance")]
        public static TProxy? ObtainProxy<TProxy>(this IProxyManager<Nothing> self, object? instance) where TProxy : class
        {
            return self.ObtainProxy<Nothing, TProxy>(instance, Nothing.AtAll, Nothing.AtAll);
        }


        /// <summary>
        /// Tries to return a proxy instance for a given instance (or unproxy it if it's already a proxy and the type would be accessible when doing so).
        /// </summary>
        /// <typeparam name="TProxy"></typeparam>
        /// <param name="self">Target of the extension method.</param>
        /// <param name="toProxy">The instance to create a proxy for (or to unproxy).</param>
        /// <param name="proxy">The resulting proxy instance (or unproxied instance), if the (un)proxying succeeds.</param>
        /// <returns>`true` if the (un)proxying succeeds, `false` otherwise.</returns>
        public static bool TryProxy<TProxy>(this IProxyManager<Nothing> self, object? toProxy, out TProxy? proxy) where TProxy : class
        {
            return self.TryProxy<Nothing, TProxy>(toProxy, Nothing.AtAll, Nothing.AtAll, out proxy);
        }
    }
}
