﻿using Salus.Models.Requests;

namespace Salus.Services.Interfaces
{
    public interface ILast24hService
    {
        public Last24h Add(AddRecipeToLast24H request);
        void Delete(int id);
        Last24h HalfPortion(int id);
        Last24h ThirdPortion(int id);
        Last24h QuarterPortion(int id);
        Last24h DoublePortion(int id);
        List<Last24h> GetAll(DateTime? dateTime);

    }
}
