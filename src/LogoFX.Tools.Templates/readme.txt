Visual Studio Extensions can be debugged like any other application. You just need to setup the debug experience to launch devenv with the loaded extension. Try the following

Right click on the project and select Properties
Go to the Debug Tab
Click on the radio button for Start External Program. Point it to the devenv.exe binary (i. e. "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe")

Then set the command line arguments to /rootsuffix Exp. This tells Visual Studio to use the experimental hive instead of the normal configuration hive. By default VSIX extensions when built will register themselves in the experimental hive.

Now you can F5 and it will start Visual Studio with your VSIX as an available extension.