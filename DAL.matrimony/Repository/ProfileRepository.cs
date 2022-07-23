using Dapper.Infrastructure;
using matrimony.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace matrimony.repository.Repository
{
    public class ProfileRepository : IGenericRepository<Profile>
    {
        public Task<int> AddAsync(Profile entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Profile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Profile> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Profile entity)
        {
            throw new NotImplementedException();
        }
    }
}
