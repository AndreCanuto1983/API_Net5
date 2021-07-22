using System.Collections.Generic;

namespace Domain.Interfaces.Service
{
    public interface IResponse<T>
    {
        bool success { get; set; }
        T response { get; set; }
    }

    public interface IResponseList<T>
    {
        bool success { get; set; }
        IEnumerable<T> response { get; set; }
    }
}
