using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using System.IO;

namespace DancingGoat.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRewriter(this IApplicationBuilder app, string path)
        {
            using var urlRewriteStreamReader = File.OpenText(path);
            var options = new RewriteOptions().AddIISUrlRewrite(urlRewriteStreamReader);
            app.UseRewriter(options);
        }
    }
}
