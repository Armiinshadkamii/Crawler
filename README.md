# what is a crawler?
In the spirit of data gathering, a crawler, also known as a web spider is a bot that
visits web pages and extracts all of the links in that web page.
# How does this crawler work?

After exracting all the links of a root url ( where the crawler starts crawling ), it
doesnt stop there! it visits all of the previously extracted links and crawls them as
well. The same process is repeated for all of the extracted links until the user reaches
to a depth or the web page runs out of links. the extracted links are unique.
# Adjustments and Settings

* the ability to specify a depth ( not for the extracted links, but for the urls that the
bot will visit ) or choose to crawl without any depth at all.
* Crawling the links that are of the same domain as the root or crawling all of the links
found on a page.
* Setting a dealy time after a specified number of requests.
* Setting the number of Get requests before delay.
* Setting a time out duration for http get request.
* Specifying file path and file name.
* Choosing a save method.
# Save methods

### Tree
when you choose tree as your method of saving, you can see all of the links in a parent
child manner. Meaning that for each link, you can see the parent link and also the children
links.
### List
In this method of saving, parent and children of links are not shown. you just get a list of found
links.
#### The output format for both saving methods is in xml.
