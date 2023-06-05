using Bogus;
using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;

namespace Services.Bogus
{
    public class CrudService<T> : ICrudService<T> where T : Entity
    {
        protected ICollection<T> Entities { get; }
        public CrudService(BaseFaker<T> faker) : this(faker, new Random().Next(1, 100))
        {
        }
        public CrudService(BaseFaker<T> faker, int count)
        {
            Entities = faker.Generate(count);
        }


        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Entities.Max(x => x.Id) + 1;
            Entities.Add(entity);
            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await ReadAsync(id);
            if(entity is not null)
                Entities.Remove(entity);
        }

        public Task<T?> ReadAsync(int id)
        {
            return Task.FromResult(Entities.SingleOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(Entities.ToList().AsEnumerable());
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var localEntity = await ReadAsync(id);
            if (localEntity is null)
            {
                return;
            }
            Entities.Remove(localEntity);
            entity.Id = id;
            Entities.Add(entity);

        }
    }
}