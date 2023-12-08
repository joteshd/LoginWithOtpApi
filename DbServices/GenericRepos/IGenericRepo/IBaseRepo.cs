using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.GenericRepos.IGenericRepo
{
    public interface IBaseRepo<T> where T : class
    {
       bool Create(T entity);
      
       bool Update(T entity);
      
       T SelectById(long Id);

        IQueryable<T> getWithCondition(object where, string[] Includecolunms = null);

        
    }
}
