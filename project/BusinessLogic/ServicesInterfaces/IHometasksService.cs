using System.Collections.Generic;

namespace BusinessLogic
{
    public interface IHometasksService
    {
        int Create(HometaskDto hometaskDto);
        HometaskDto Get(int hometaskId);
        IReadOnlyCollection<HometaskDto> GetAll();
        int Update(int hometaskId, HometaskDto hometaskDto);
        void Delete(int hometaskId);
    }
}