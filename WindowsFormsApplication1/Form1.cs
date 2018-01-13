using System;
using System.Windows.Forms;
using OSGeo.OGR;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            changeFile(@"C:\uploadShp\1515737858878\bou2_4p.shp");
        }
        public static bool changeFile(string filepath)
        {
            // 为了支持中文路径，请添加下面这句代码
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");
           
            Ogr.RegisterAll();
            DataSource ds = Ogr.Open(filepath, 0);
            if (ds == null)
            {
                MessageBox.Show("打开文件失败！");
                return false;
            }
            MessageBox.Show("打开文件成功！");
            Driver dv = Ogr.GetDriverByName("GeoJSON");
            if (dv == null)
            {
                MessageBox.Show("打开驱动失败！");
                return false;
            }
            MessageBox.Show("打开驱动成功！");
            dv.CopyDataSource(ds, "C://changeShp//a.geojson", null);
            MessageBox.Show("数据转换成功！");
            return true;
        }
    }
}
