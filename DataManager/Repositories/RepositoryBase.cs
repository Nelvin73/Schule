
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using Groll.Schule.Datenbank;

namespace Groll.Schule.DataManager.Repositories
{
    public class RepositoryBase<T> where T : class, new()
        {
            protected SchuleContext context;

            public RepositoryBase(SchuleContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                this.context = context;
                context.Set<T>().Load();
            }

            public T GetById(int Id)
            {              
                T data = context.Set<T>().Find(Id);
                return data;
            }

            public T Create()
            {
                return context.Set<T>().Create();
                return context.Set<T>().Add(new T());
            }

        public void Delete(T t)
            {
                try
                {
                    context.Set<T>().Remove(t);
                }
            catch (Exception e)
                {
                    throw;
                }

            }

            public T Add(T t)
            {
                try
                { 
                    return context.Set<T>().Add(t);
                }
                catch (Exception e)
                {
                    
                    throw;
                }
               
            }

            public List<T> GetList()
            {
                return context.Set<T>().ToList();
            }

            public ObservableCollection<T> GetObservableCollection()
            {
                return context.Set<T>().Local;
            }

            public List<T> GetList(System.Linq.Expressions.Expression<Func<T, bool>> filter)
            {
                return context.Set<T>().Where(filter).ToList();
            }

            public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter)
            {
                return context.Set<T>().Where(filter).FirstOrDefault();
            }

            public void Save()
            {
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    throw;
                }
            }

        }

}
