using System.Configuration;
using System.Web;
using Faust.Andrew.LiteWiki.DataAccess;
using System.Web.Configuration;

namespace ZJU.COPLib
{
    public class WikiOperations
    {
        private const string DBName = "WikiDB";
        private const string DBSchemaKey = "WikiSchema";
        private const string DBType = "Oracle";
        private const string Schema = "copa2";
        private static WikiDataHandler GetDataHandler()
        {
            WikiDataHandler handler ;
            if (DBType == "MySql")
                handler = new WikiMySqlDataHandler();
            else
                handler = new WikiSqlDataHandler();
            handler.ConnectionString = WebConfigurationManager.ConnectionStrings["COPaWiKi"].ConnectionString; 
            handler.Schema = Schema ;
            return handler;
        }

        public static bool IsPagePrivate(string PageName)
        {   if (!DoesPageExist(PageName))
            {
                return false;
            }

            return GetDataHandler().IsPagePrivate(PageName);
        }

        public static bool AllowAnonymousEdit(string PageName)
        {            
            return GetDataHandler().AllowAnonymousEdit(PageName);
        }

        public static bool DoesPageExist(string PageName)
        {
            return GetDataHandler().DoesPageExist(PageName);
        }

        public static int UpdatePage(WikiPage wikiPage)
        {            
            return GetDataHandler().UpdatePage(wikiPage);
        }

        public static WikiPage GetWikiPage(string PageName)
        {
            return GetDataHandler().GetPage(PageName);
        }

        public static int CreateWikiPage(WikiPage wikiPage)
        {
            return GetDataHandler().CreatePage(wikiPage);
        }

        public static WikiPageHistory[] GetWikiPageHistory(string PageName)
        {
            return GetDataHandler().GetPageHistory(PageName);
        }

        public static string GetFullApplicationPath(HttpRequest Request)
        {
            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;
        }
    }
}
