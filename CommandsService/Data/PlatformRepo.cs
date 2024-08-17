using CommandsService.Models;

namespace CommandsService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDBContext _dbContext;

        public PlatformRepo(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
                throw new ArgumentNullException(nameof(plat));
            
            _dbContext.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _dbContext.Platforms.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }

        public bool IsPlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(x => x.Id == platformId);
        }

        public bool IsPlatformExistsByExternalId(int externalid)
        {
            return _dbContext.Platforms.Any(x => x.ExternalId == externalid);
        }
    }
}