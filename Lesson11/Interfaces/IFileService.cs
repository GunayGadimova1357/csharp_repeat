using Lesson11.Data.Model;

namespace Lesson11.Interfaces;

public interface IFileService
{
    void Save (Results results);
    void DeleteMovie (string movieName);
}