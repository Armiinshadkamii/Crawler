using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crawler.UriOps;
using crawler.DataSets;

namespace crawler.SaveMethods;
public class TreeMethod
{
    Settings settings;

    public TreeMethod(Settings s)
    {
        settings = s;

        UniformResourceIdentifier.SetTimeOut(settings.GetTimeout());
    }

    public async Task<HashSet<Tree>> CrawlAsync(string seedUrl)
    {
        settings.AlreadyCrawled.Add(seedUrl.ToString());

        if (UniformResourceIdentifier.TryParse(seedUrl) == false)
        {
            HashSet<Tree> toReturn = new HashSet<Tree>();

            return toReturn;
        }
        Uri uri = new Uri(seedUrl);

        bool isValid = await UniformResourceIdentifier.ValidateAsync(uri);

        if (isValid)
        {
            string? htmlBody = await UniformResourceIdentifier.GetHtmlBodyAsync(seedUrl);

            HashSet<string> freshLinks = UniformResourceIdentifier.GetLinks(settings.GetUri(), htmlBody)
                .Except(settings.AlreadyCrawled).Except(settings.RetrievedChildren).ToHashSet();
            settings.RetrievedChildren.UnionWith(freshLinks);

            if (freshLinks.Count == 0) // Base case 3
            {
                HashSet<Tree> toReturn = new HashSet<Tree>();

                return toReturn;
            }
            else
            {
                HashSet<Tree> linksTree = new HashSet<Tree>();

                // To Hashset will creare a new hashset for each parent url and initializes our object
                // with that. this is to prevent modifications from being stored.

                foreach (var link in freshLinks.ToHashSet())
                {
                    if (settings.GetDeepCrawl() == false && settings.AlreadyCrawled.Count >= settings.GetDepth()) // No more than n Pages
                    {
                        // if the program has reached to
                        // a specified depth you should 
                        // still handle the remaining items
                        // in the list of links form previous
                        // calls. but you shouldnt cawl them.
                        Tree tree = new Tree(seedUrl, link);
                        linksTree.Add(tree);
                    }
                    else
                    {
                        if (settings.AlreadyCrawled.Count >= settings.GetNextDelayAt() && settings.AlreadyCrawled.Count != settings.GetDepth())
                        {
                            settings.Rest();
                        }

                        if (settings.IfDomainSpecific())
                        {
                            if (UniformResourceIdentifier.IsOfTheSameHost(settings.GetUri(), link))
                            {
                                Console.WriteLine(link);

                                Tree tree = new Tree(seedUrl, link, await CrawlAsync(link));
                                linksTree.Add(tree);
                            }
                            else
                            {
                                // if a node is not of the same
                                // host as root, then you shouldnt
                                // crawl it. but you still need to
                                // create an object form it and show
                                // in the output.

                                ConsoleDataDisplay.PrintWarning("-> Not of the same host <-");
                                Console.WriteLine(link);
                                Tree tree = new(seedUrl, link);
                                linksTree.Add(tree);
                            }
                        }
                        else
                        {
                            // if it is not domain specific,
                            // you dont have to check for it
                            // being domain specific.

                            Console.WriteLine(link);

                            Tree tree = new Tree(seedUrl, link, await CrawlAsync(link));
                            linksTree.Add(tree);
                        }

                    }

                }
                return linksTree;
            }
        }
        else // Base Case 2
        {
            HashSet<Tree> toReturn = new HashSet<Tree>();

            return toReturn;
        }

    }
}

