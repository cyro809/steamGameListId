using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
namespace ConsoleApplication1
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            Teste t = new Teste();
            t.getList();
        }
    }
    class Teste
    {
        public HtmlDocument doc = new HtmlDocument();
        public string titulo;
        public List<string> tags = new List<string>();
        public string appDetails;
        public string appList;
        public void getList()
        {
            using (WebClient client = new WebClient())
            {
                appList = client.DownloadString("http://api.steampowered.com/ISteamApps/GetAppList/v0001/");
            }
            JObject jobject = JObject.Parse(appList);
            JObject apps = (JObject)jobject["applist"];
            JObject list = (JObject)apps["apps"];
            JArray app = (JArray)list["app"];
            for (int i=2980;i<app.Count;i++)
            {
                JObject game = (JObject)app[i];
                int appId = (int)game["appid"];
                Console.WriteLine(appId);
                if(appId > 294230)
                {
                    pesquisarTitulo(appId);
                }
                
            }
        }
        public void pesquisarTitulo(int id)
        {
            string gameInfoJson;
            Jogo game = new Jogo();
            using (WebClient client = new WebClient())
            {

                gameInfoJson = client.DownloadString(@"http://store.steampowered.com/api/appdetails/?appids=" + id.ToString());

                JObject jObject = JObject.Parse(gameInfoJson);
                JObject idObject = (JObject)jObject[id.ToString()];
                bool success = (bool)idObject["success"];
                if (success)
                {
                    JObject dataObject = (JObject)idObject["data"];


                    //game.steam_appid = (string)dataObject["steam_appid"];
                    game.type = (string)dataObject["type"];
                    if (game.type == "game")
                    {
                        using (StreamWriter arquivo = File.AppendText(@"gameList.txt"))
                        {
                            arquivo.WriteLine(id);
                        }
                    }
                }
                
                //AppDetails
            }
        }
    }
    public class Jogo
    {
        public string steam_appid {get;set;}
        public string name { get; set; }
        public string type { get; set; }
        public Price_overview price_overview { get; set; }
        public string controller_support { get; set; }
        public Platforms platforms { get; set; }
        public string[] developers { get; set; }
        public string[] publishers { get; set; }
        public Categories[] categories { get; set; }
        public Recommendations recommendations { get; set; }
        public MetaCritic metacritic { get; set; }
    }
    public class Price_overview
    {
        public string currency { get; set; }
        public int final { get; set; }
    }
    public class Platforms
    {
        public bool windows { get; set; }
        public bool mac { get; set; }
        public bool linux { get; set; }
    }
    public class Categories
    {
        public int id { get; set; }
        public string description { get; set; }
    }
    public class Recommendations
    {
        public int total { get; set; }
    }
    public class MetaCritic
    {
        public int score;
    }
}
