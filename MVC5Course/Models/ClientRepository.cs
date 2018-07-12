using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Course.Models
{
    public class ClientRepository : EFRepository<Client>, IClientRepository
    {
        //16 �Эק� ClientsController ���R���\��A����Ʈw�u�Хܤw�R���v�Y�i�A���n�u���R�����
        public IQueryable<Client> All (bool isAll = false)
        {
            
            if (isAll)
            {
                return base.All();
            }

            return base.All().Where(p => p.CreditRating < 6 && p.IsDeleted == false);
        }

        public IQueryable<Client> SearchKeyword(string keyword, int take)
        {
            var data = this.All();

            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(w => w.FirstName.Contains(keyword));
            }

            if (take > 0)
            {
                data = data.OrderByDescending(o => o.ClientId).Take(take);
            }
            else
            {
                data = data.OrderByDescending(o => o.ClientId).Take(5);
            }

            return data;
        }

        internal Client Find(int id)
        {
            return this.All().FirstOrDefault(f => f.ClientId == id);
        }

        public override void Delete(Client entity)
        {
            entity.IsDeleted = true;
        }
    }

    public interface IClientRepository : IRepository<Client>
    {

    }
}