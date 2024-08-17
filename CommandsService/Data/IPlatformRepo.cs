using CommandsService.Models;

namespace CommandsService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int idplatformId);
        void CreatePlatform(Platform plat);
        bool IsPlatformExists(int platformId);
        bool IsPlatformExistsByExternalId(int externalid);
    }
}