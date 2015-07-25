# The Tools Solution  [![Build status](https://ci.appveyor.com/api/projects/status/3fvypqbkemhyc5gm/branch/master?svg=true)](https://ci.appveyor.com/project/glyad/logofx/branch/master)
The Tools solution purposes creation of usefull add-ins for Visual Studio aiming rapid development of LogoFX's based projects.
## How to add new Item Template to VSIX project
* Add new project of type "C# Item Template", which is located in the Extensibility section. The project contains Class1.cs and *.vstemaplte files
* Edit the template projects:
  * Remove or rename the "Class1.cs" file
  * Add another project items, if need. For example: we renamed Class1.cs to ViewModel.cs and added ViewModel.resources.cs, edited content of files, changed BuildAction attribute to None (with the Property Grid)
* Add new asset to VSIX project's assets
  * The type is Item Template
  * The Source is Project n Solution
  * The Project is the same project, which was created in previous step
