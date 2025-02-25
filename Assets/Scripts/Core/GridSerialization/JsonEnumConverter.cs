using System;
using Core.GridPawns.Enum;

namespace Core.GridSerialization
{
    public static class JsonEnumConverter
    {
        public static Enum ConvertJsonToPawnType(string pawnString)
        {
            switch (pawnString)
            {
                case nameof(JsonGridPawnType.empty):
                    return ApplianceType.None;
                case nameof(JsonGridPawnType.applianceA):
                    return ApplianceType.ApplianceA;
                case nameof(JsonGridPawnType.producerA):
                    return ProducerType.ProducerA;
                default:
                    return ApplianceType.None;
            }
        }
        
        public static JsonGridPawnType ConvertPawnTypeToJson(Enum gridPawnType)
        {
            switch (gridPawnType)
            {
                case ApplianceType.ApplianceA:
                    return JsonGridPawnType.applianceA;
                case ProducerType.ProducerA:
                    return JsonGridPawnType.producerA;
                default:
                    return JsonGridPawnType.empty;
            }
        }
    }
}
