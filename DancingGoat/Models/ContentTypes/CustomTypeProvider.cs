using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Models
{
    public class CustomTypeProvider : ITypeProvider
    {
        protected static readonly Dictionary<Type, string> Codenames = new Dictionary<Type, string>
        {
            {typeof(AboutUs), "about_us"},
            {typeof(Accessory), "accessory"},
            {typeof(Article), "article"},
            {typeof(Brewer), "brewer"},
            {typeof(Cafe), "cafe"},
            {typeof(Coffee), "coffee"},
            {typeof(FactAboutUs), "fact_about_us"},
            {typeof(Grinder), "grinder"},
            {typeof(HeroUnit), "hero_unit"},
            {typeof(Home), "home"},
            {typeof(HostedVideo), "hosted_video"},
            {typeof(Office), "office"},
            {typeof(Tweet), "tweet"}
        };

        public virtual Type GetType(string contentType)
        {
            return Codenames.Keys.FirstOrDefault(type => GetCodename(type).Equals(contentType));
        }

        public virtual string GetCodename(Type contentType)
        {
            return Codenames.TryGetValue(contentType, out var codename) ? codename : null;
        }
    }
}