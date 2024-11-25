using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace crawler;
public class Settings
{
    // Fields and Setters and Getters
    Uri url;

    public Uri GetUri() { return url; }

    public HashSet<string> AlreadyCrawled = new HashSet<string>();
    public HashSet<string> GetCrawledLinks()
    {
        return AlreadyCrawled;
    }

    bool DeepCrawl = false;
    public void IsDeepCrawl(bool deepCrawl)
    {
        DeepCrawl = deepCrawl;
    }
    public bool GetDeepCrawl() { return DeepCrawl; }

    int Depth = 50;
    public void SetDepth(int _dep)
    {
        Depth = _dep;
    }
    public int GetDepth() { return Depth; }

    bool DomainSpecific = true;
    public void IsDomainSpecific(bool _isDomainSpecific)
    {
        DomainSpecific = _isDomainSpecific;
    }
    public bool IfDomainSpecific()
    {
        return DomainSpecific;
    }

    int RequestsBeforeDelay = 50;
    int NextDelayAt = 0;
    public void SetRequestsBeforeDelay(int _nextDelayFlag)
    {
        RequestsBeforeDelay = _nextDelayFlag;
        NextDelayAt = _nextDelayFlag;
    }
    public int GetNextDelayAt() { return NextDelayAt; }

    int Delay = 30_000;
    public void SetDelay(int _delay)
    {
        Delay = _delay;
    }

    int TimeOut = 10;
    public void SetTimeout(int _timeout) { TimeOut = _timeout; }
    public int GetTimeout() { return TimeOut; }

    public HashSet<string> RetrievedChildren = new HashSet<string>();

    public int GetTotalLinks()
    {
        return RetrievedChildren.Count;
    }

    public void Init()
    {
        // request timeout

        this.url = new Uri(AskUser.AskUrl("Root URI:"));

        int timeout = AskUser.AskTimeOut("Time out: (in seconds)");
        SetTimeout(timeout);
        //_httpClient.Timeout = TimeSpan.FromSeconds(timeout);

        int dep = AskUser.AskDepth(msg: "Set a depth: ( 0 for Deep crawl)");

        if (dep == 0)
        {
            DeepCrawl = true;
        }
        else
        {
            this.Depth = dep;
        }

        this.RequestsBeforeDelay = AskUser.AskRequestsBeforeDelay("requests before each delay:");
        NextDelayAt = this.RequestsBeforeDelay;

        this.Delay = AskUser.AskDelay("Delay in miliseconds:");

        this.DomainSpecific = AskUser.AskDomainSpecific("Is Domain Specific:");
    }

    public void Rest()
    {
        NextDelayAt = AlreadyCrawled.Count + RequestsBeforeDelay;

        ConsoleDataDisplay.PrintWarning($"Rest Period of target server for {Delay} miliseconds");

        Thread.Sleep(Delay);

        ConsoleDataDisplay.PrintWarning($"Crawler in action again...");
    }
}

