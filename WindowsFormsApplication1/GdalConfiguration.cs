using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;

public static partial class GdalConfiguration
{
    private static bool _configuredOgr;
    private static bool _configuredGdal;

    /// <summary>  
    /// Function to determine which platform we're on  
    /// </summary>  
    private static string GetPlatform()
    {
        return IntPtr.Size == 4 ? "x86" : "x64";
    }
    static GdalConfiguration()
    {
        MessageBox.Show("开始");
        var executingAssemblyFile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        var executingDirectory = Path.GetDirectoryName(executingAssemblyFile);

        if (string.IsNullOrEmpty(executingDirectory))
            throw new InvalidOperationException("cannot get executing directory");


        var gdalPath = Path.Combine(executingDirectory, "gdal");
        var nativePath = Path.Combine(gdalPath, GetPlatform());

        // Prepend native path to environment path, to ensure the  
        // right libs are being used.  
        var path = Environment.GetEnvironmentVariable("PATH");
        
        path = nativePath + ";" + Path.Combine(nativePath, "plugins") + ";" + path;
        Console.WriteLine(path);
        Environment.SetEnvironmentVariable("PATH", path);

        // Set the additional GDAL environment variables.  
        var gdalData = Path.Combine(gdalPath, "data");
        Environment.SetEnvironmentVariable("GDAL_DATA", gdalData);
        Gdal.SetConfigOption("GDAL_DATA", gdalData);

        var driverPath = Path.Combine(nativePath, "plugins");
        Environment.SetEnvironmentVariable("GDAL_DRIVER_PATH", driverPath);
        Gdal.SetConfigOption("GDAL_DRIVER_PATH", driverPath);

        Environment.SetEnvironmentVariable("GEOTIFF_CSV", gdalData);
        Gdal.SetConfigOption("GEOTIFF_CSV", gdalData);

        var projSharePath = Path.Combine(gdalPath, "share");
        Environment.SetEnvironmentVariable("PROJ_LIB", projSharePath);
        Gdal.SetConfigOption("PROJ_LIB", projSharePath);
        MessageBox.Show("初始化完毕！");
    }

    /// <summary>  
    /// Method to ensure the static constructor is being called.  
    /// </summary>  
    /// <remarks>Be sure to call this function before using Gdal/Ogr/Osr</remarks>  
    public static void ConfigureOgr()
    {
        MessageBox.Show("配置Ogr。。。。。");
        if (_configuredOgr) return;

        // Register drivers  
        Ogr.RegisterAll();
        _configuredOgr = true;
    }
    public static void ConfigureGdal()
    {
        MessageBox.Show("配置Gdal。。。。。");
        if (_configuredGdal) return;

        // Register drivers  
        Gdal.AllRegister();
        _configuredGdal = true;
    }
}
