using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Identity.Service.OAuth
{
    public class ClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var memoryClients = OAuthMemoryData.GetClients();
            if (memoryClients.Any(oo => oo.ClientId == clientId))
            {
                return memoryClients.FirstOrDefault(oo => oo.ClientId == clientId);
            }

            return GetClient(clientId);
        }

        private Client GetClient(string client)
        {
            //TODO 根据数据库查询
            return null;
        }
    }
}
