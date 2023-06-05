using Bogus;
using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;

namespace Services.Bogus
{
    public class CrudService<T> : ICrudService<T> where T : Entity
    {
        private ICollection<T> _entities;
        public CrudService(BaseFaker<T> faker) : this(faker, new Random().Next(1, 100))
        {
        }
        public CrudService(BaseFaker<T> faker, int count)
        {
            _entities = faker.Generate(count);
        }


        public Task<T> CreateAsync(T entity)
        {
            entity.Id = _entities.Max(x => x.Id) + 1;
            _entities.Add(entity);
            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await ReadAsync(id);
            if(entity is not null)
                _entities.Remove(entity);
        }

        public Task<T?> ReadAsync(int id)
        {
            return Task.FromResult(_entities.SingleOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(_entities.ToList().AsEnumerable());
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var localEntity = await ReadAsync(id);
            if (localEntity is null)
            {
                return;
            }
            _entities.Remove(localEntity);
            entity.Id = id;
            _entities.Add(entity);

        }
    }
}