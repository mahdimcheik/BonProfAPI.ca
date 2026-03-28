namespace BonProfCa.Utilities;

public static class GeoHelper
{
    public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; // Rayon de la terre en km
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c; // Distance en km
        
        return d;
    }
    
    public static (double minLat, double maxLat, double minLon, double maxLon) GetBounders(double myLat, double myLon, double distanceKm = 10)
    {
        // 1. Approximation : 1 degré ~ 111 km
        // Pour la latitude, c'est constant
        double deltaLat = distanceKm / 111.0;

        // Pour la longitude, cela dépend de la latitude actuelle (il faut convertir en radians pour le Cos)
        // On utilise Math.Cos pour ajuster l'écartement des méridiens
        double latInRadians = myLat * (Math.PI / 180.0);
        double deltaLon = distanceKm / (111.0 * Math.Cos(latInRadians));

        // 2. Calcul des bornes
        double minLat = myLat - deltaLat;
        double maxLat = myLat + deltaLat;
        double minLon = myLon - deltaLon;
        double maxLon = myLon + deltaLon;
        
        return (minLat, maxLat, minLon, maxLon);
    }

    private static double ToRadians(double angle)
    {
        return Math.PI * angle / 180.0;
    }
}