﻿using Domain.Objects.Reponses;
using Domain.Objects.Requests;

namespace Domain.Interfaces.Services
{
    public interface IProcedureService
    {
        Task DeleteProcedure(int id);
        ICollection<ProcedureResponse> GetProcedures(int currentPage, int takeQuantity);
        Task SaveProcedure(CreateProcedureRequest createProcedureRequest);
        Task UpdateProcedure(int id, UpdateProcedureRequest updateProcedureRequest);
    }
}
