using crawler;
using crawler.DataSets;
using crawler.SaveMethods;
using crawler.UriOps;
using System.Text;
using System.Xml;

class Program
{
    static Uri url;
    static Settings settings ;

    enum Stage
    {
        Init = 0,
        Method = 1,
    };

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        
        int stage = (int)Stage.Init;
        while (true)
        {
            if (stage == (int)Stage.Init)
            {
                ConsoleDataDisplay.PrintWarning("Initialization Stage!");
                settings = new Settings();
                
                settings.Init();
                url = settings.GetUri();

                stage = (int)Stage.Method;
            }
            else if (stage == (int)Stage.Method)
            {
                int method = 0;

                method = AskUser.AskDataStructure("How to save data? (1 or 2)\n1. Single list\n2. Tree list");

                try
                {

                    switch (method)
                    {
                        case 1:
                            ConsoleDataDisplay.PrintWarning("Save as:");

                            string path = AskUser.AskFilePath("File Path:");
                            string name1 = AskUser.AskFileName("File Name:");


                            var watch = System.Diagnostics.Stopwatch.StartNew();

                            ListMethod listMethod = new ListMethod(settings);
                            HashSet<SingleList> singleLists = await listMethod.SingleListCrawlAsync(url.ToString());

                            watch.Stop();

                            ConsoleDataDisplay.PrintSuccess($"\nExecute time -> {watch.ElapsedMilliseconds / 1000} seconds");
                            ConsoleDataDisplay.PrintSuccess($"Total links found -> {settings.GetTotalLinks()}");

                            XOutput.MakeXmlList(settings.GetUri().ToString(), singleLists, path, name1);

                            break;
                        case 2:
                            ConsoleDataDisplay.PrintWarning("Save as:");

                            string filePath = AskUser.AskFilePath(msg: "File Path:");
                            string name = AskUser.AskFileName("File name:");

                            var watch2 = System.Diagnostics.Stopwatch.StartNew();

                            TreeMethod treeMethod = new TreeMethod(settings);
                            HashSet<Tree> treeList = await treeMethod.CrawlAsync(url.ToString());

                            watch2.Stop();

                            ConsoleDataDisplay.PrintSuccess($"\nExecute time -> {watch2.ElapsedMilliseconds / 1000} seconds");
                            ConsoleDataDisplay.PrintSuccess($"Total links found -> {settings.GetTotalLinks()}");

                            XOutput.MakeXmlTree(settings.GetUri().ToString(), treeList, filePath, name);

                            break;
                    }
                }
                catch (Exception e)
                {
                    ConsoleDataDisplay.PrintError(e.Message);
                }

                stage = (int)Stage.Init;
            }
        }
    }
}