public struct GeoCoord
{
    public GeoCoord(double longitude, double latitude, double elevation = 0)
    {
        Longitude = longitude;
        Latitude = latitude;
        Elevation = elevation;
    }
    
    public readonly double Longitude;
    public readonly double Latitude;
    public double Elevation;
}