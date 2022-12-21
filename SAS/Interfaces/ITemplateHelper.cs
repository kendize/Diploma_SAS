using System.Threading.Tasks;

namespace SAS.Interfaces
{
    public interface ITemplateHelper
    {
        Task<string> GetTemplateHtmlAsStringAsync<T>(
                              string viewName, T model);
    }
}
