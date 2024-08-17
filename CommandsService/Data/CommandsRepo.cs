using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandsRepo : ICommandsRepo
    {
        private readonly AppDBContext _dbContext;
        public CommandsRepo(AppDBContext context)
        {
            _dbContext = context;
        }

        public void CreateCommand(int platformId, Commands command)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));
            
            command.PlatformId = platformId;
            _dbContext.Commands.Add(command);
        }

        public IEnumerable<Commands> GetAllCommands()
        {
            return _dbContext.Commands.ToList();
        }

        public IEnumerable<Commands> GetAllCommandsByPlatformId(int platformId)
        {
            return _dbContext.Commands
                .Where(x => x.PlatformId == platformId)
                .OrderBy(x => x.Plaatform.Name)
                .ThenBy(x => x.HowTo)
                .ToList();
        }

        public Commands GetCommandById(int platformId, int id)
        {
            return _dbContext.Commands.FirstOrDefault(x => x.PlatformId == platformId && x.Id == id);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}