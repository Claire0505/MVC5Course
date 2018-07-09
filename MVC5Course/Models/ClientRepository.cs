using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class ClientRepository : EFRepository<Client>, IClientRepository
    {
        //16 �Эק� ClientsController ���R���\��A����Ʈw�u�Хܤw�R���v�Y�i�A���n�u���R�����
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