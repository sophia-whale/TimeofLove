using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace TabbedTemplate.Services
{
    public interface IHoneyWordsService
    {
        public Task<string> GetHoneyWordsAsync();
    }
}
