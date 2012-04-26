namespace Simple.Web.CodeGeneration
{
    static class WriteView
    {
        public static void Impl(ISpecifyView endpoint, IContext context)
        {
            WriteUsingContentTypeHandler(endpoint, context);
        }

        private static void WriteUsingContentTypeHandler(ISpecifyView endpoint, IContext context)
        {
            IContentTypeHandler contentTypeHandler;
            if (TryGetContentTypeHandler(context, out contentTypeHandler))
            {
                context.Response.ContentType = contentTypeHandler.GetContentType(context.Request.AcceptTypes);
                var content = new Content(endpoint, null, endpoint.ViewPath);
                contentTypeHandler.Write(content, context.Response.Output);
            }
        }

        private static bool TryGetContentTypeHandler(IContext context, out IContentTypeHandler contentTypeHandler)
        {
            try
            {
                contentTypeHandler = new ContentTypeHandlerTable().GetContentTypeHandler(context.Request.AcceptTypes);
            }
            catch (UnsupportedMediaTypeException)
            {
                context.Response.StatusCode = 415;
                context.Response.StatusDescription = "Unsupported media type requested.";
                contentTypeHandler = null;
                return false;
            }
            return true;
        }
    }
}