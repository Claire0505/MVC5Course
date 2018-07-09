using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class ClientRepository : EFRepository<Client>, IClientRepository
    {
        //16 請修改 ClientsController 的刪除功能，讓資料庫「標示已刪除」即可，不要真的刪除資料
        public IQueryable<Client> All (bool isAll = false)
        {
            var data = base.All();

            if (!isAll)
            {
                data = base.All().Where(p => p.CreditRating < 2 && p.Active == false);
            }

            return data;
        }

        public IQueryable<Client> SearchKeyword(string keyword)
        {
            var data = this.All();

            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(w => w.FirstName.Contains(keyword));
            }

            return data;
        }

        internal Client Find(int id)
        {
            return this.All().FirstOrDefault(f => f.ClientId == id);
        }

        public override void Delete(Client entity)
        {
            entity.Active = false;
        }
    }

    public interface IClientRepository : IRepository<Client>
    {

    }
}