using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandsRepo
    {
        bool SaveChanges();
        IEnumerable<Commands> GetAllCommands();
        IEnumerable<Commands> GetAllCommandsByPlatformId(int id);
        Commands GetCommandById(int platformId,int id);
        void CreateCommand(int platformId, Commands command);
    }
}