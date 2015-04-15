Doc-As-Code
===========
Easily build and publish your API documentation. We currently support C# and VB projects.

Getting started
---------------
As a prerequisite, you will need either VS 2015 or VS 2013 with Roslyn compiler installed.
* Install "docascode.msi" from (binary location TBD)
  * Alternatively if you've cloned our source code can grab the build output from \DocAsCode\Drop\Debug\Installer\docascode.msi
* Create new project
* Select Installed > Templates > Visual C# on the left pane, and scroll down to DocumentationWebsite in the center pane
* Add a source project to References (all your triple slash comments will be imported to the final website)
* Build / view in browser (CTRL + SHIFT + W)

Note that if you are just adding to table of contents or authoring in markdown, you can simply refresh the page and the changes should take effect. However, you will need to rebuild if you are adding a new markdown file to the API reference section.

Adding conceptual content
-------------------------
The components of the project you care about now are as follows:
```
api/
article/
  about.md
  index.md
toc.yaml
```
* api/ is reserved for your API section. **in the future we will add an index.md file so you can customize the root page of the API reference**
* article/ is where you put all of the self authored topics.
* toc.yaml is the main table of contents file that generates the navbar you will see on the top. It looks like this:
```
- id: Home
  href: articles/index.md
- id: About
  href: articles/about.md
- id: Api Documentation
  href: api
```
Say I want to add another tutorial section, here's how I would go about it

Understanding the URL
---------------------
