using System.Threading.Tasks;
using ZxInfoBot.publish.TgBot;

namespace ZxInfoBot
{
    
    //   dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
    
    class Program
    {
        
        static async Task Main(string[] args)
        {
            await Bot.StartBot();
            // var model =await Api.GetHitByName("yanga");
        }
    }
}

