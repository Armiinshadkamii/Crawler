using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crawler.UriOps;
using crawler.DataSets;

namespace crawler.SaveMethods;
public class ListMethod
{
    Settings settings;

    public ListMethod(Settings s)
    {
        settings = s;
    }

    public async Task<HashSet<SingleList>> SingleListCrawlAsync(string seed)
    {
        settings.AlreadyCrawled.Add(seed);

        try
        {
            Uri uri = new Uri(seed);

            if (await UniformResourceIdentifier.ValidateAsync(uri))
            {
                try
                {
                    string body = await UniformResourceIdentifier.GetHtmlBodyAsync(seed);

                    HashSet<string> children = UniformResourceIdentifier.GetLinks(settings.GetUri(), body)
                        .Except(settings.RetrievedChildren).Except(settings.AlreadyCrawled).ToHashSet();
                    settings.RetrievedChildren.UnionWith(children);

                    if (children.Count != 0)
                    {
                        HashSet<SingleList> nodesHash = new HashSet<SingleList>();
                        foreach (var child in children)
                        {
                            // in order to return all of the
                            // found links inside a url, we
                            // first create an object from 
                            // all of those links regardless
                            // of them being of the same domain
                            // or not. if we dont do this those
                            // links will be lost and wont be 
                            // returned which is not what we want
                            // in this app.

                            SingleList singleList = new SingleList(node: child, parent: seed);

                            nodesHash.Add(singleList);
                        }

                        foreach (var link in nodesHash.ToList())
                        {
                            if (settings.GetDeepCrawl() == false && settings.AlreadyCrawled.Count >= settings.GetDepth())
                            {
                                break;
                            }

                            if (settings.AlreadyCrawled.Count >= settings.GetNextDelayAt() && settings.AlreadyCrawled.Count != settings.GetDepth())
                            {
                                settings.Rest();
                            }

                            if (settings.IfDomainSpecific())
                            {
                                if (UniformResourceIdentifier.IsOfTheSameHost(settings.GetUri(), link.Node))
                                {
                                    // find all the children of links
                                    // that are of the same domain and
                                    // add them to the list.

                                    Console.WriteLine($"{link.Node}");

                                    nodesHash.UnionWith(await SingleListCrawlAsync(link.Node));
                                }
                            }
                            else
                            {
                                // find all the children of links
                                // regardless of them being domain
                                // specific or not and add them to
                                // the list.

                                ConsoleDataDisplay.PrintWarning("-> Not of the same host <-");
                                Console.WriteLine($"{link.Node}");

                                nodesHash.UnionWith(await SingleListCrawlAsync(link.Node));
                            }

                        }

                        return nodesHash;
                    }
                    else
                    {
                        return new HashSet<SingleList>();
                    }
                }
                catch (Exception e)
                {
                    ConsoleDataDisplay.PrintError(e.Message);

                    return new HashSet<SingleList>();
                }
            }
            else
            {
                return new HashSet<SingleList>();
            }
        }
        catch (Exception e)
        {
            ConsoleDataDisplay.PrintError(e.Message);

            return new HashSet<SingleList>();
        }

    }
}

